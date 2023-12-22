using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.CommandDetails;

/// <summary>
/// Create a new CommandDetail
/// </summary>
/// <remarks>
/// Creates a new command detail given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateCommandDetailRequest, CreateCommandDetailResponse>
{
  private const string EndPointId = "ENP-121";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateCommandDetailRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Command Detail End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new command detail";
      s.Description = "This endpoint is used to create a new  command detail. Here details of command detail to be created is provided";
      s.ExampleRequest = new CreateCommandDetailRequest { Action = "Action", ActiveState = "Active State", Category = "Category", CommandId = "1000", CommandName = "Command Name", CommandText = "Command Text", ImageId = string.Empty, ImagePath = "Image Path", IsEnabled = true, IsPublished = true, Narration = "Narration", ShortcutKey = "Shortcut Key" };
      s.ResponseExamples[200] = new CreateCommandDetailResponse("Action", "Active State", "Category", "1000", "Command Name", "Command Text", 0, "Image Path", true, true, "Narration", "Shortcut Key", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateCommandDetailRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<CommandDetailDTO>();
    requestDTO.Id = request.CommandId;

    var result = await mediator.Send(new CreateModelCommand<CommandDetailDTO, CommandDetail>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is CommandDetailDTO obj)
      {
        Response = new CreateCommandDetailResponse(obj.Action, obj.ActiveState, obj.Category, obj.Id, obj.CommandName, obj.CommandText, obj.ImageId, obj.ImagePath, obj.IsEnabled, obj.IsPublished, obj.Narration, obj.ShortcutKey, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
