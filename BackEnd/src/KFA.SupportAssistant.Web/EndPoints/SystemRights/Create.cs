using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.SystemRights;

/// <summary>
/// Create a new SystemRight
/// </summary>
/// <remarks>
/// Creates a new system right given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateSystemRightRequest, CreateSystemRightResponse>
{
  private const string EndPointId = "ENP-201";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateSystemRightRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add System Right End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new system right";
      s.Description = "This endpoint is used to create a new  system right. Here details of system right to be created is provided";
      s.ExampleRequest = new CreateSystemRightRequest { IsCompulsory = true, Narration = "Narration", RightId = "1000", RightName = "Right Name" };
      s.ResponseExamples[200] = new CreateSystemRightResponse(true, "Narration", "1000", "Right Name", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateSystemRightRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<SystemRightDTO>();
    requestDTO.Id = request.RightId;

    var result = await mediator.Send(new CreateModelCommand<SystemRightDTO, SystemRight>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is SystemRightDTO obj)
      {
        Response = new CreateSystemRightResponse(obj.IsCompulsory, obj.Narration, obj.Id, obj.RightName, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
