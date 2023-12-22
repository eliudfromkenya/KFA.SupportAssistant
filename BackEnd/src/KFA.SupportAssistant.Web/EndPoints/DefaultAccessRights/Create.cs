using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.DefaultAccessRights;

/// <summary>
/// Create a new DefaultAccessRight
/// </summary>
/// <remarks>
/// Creates a new default access right given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateDefaultAccessRightRequest, CreateDefaultAccessRightResponse>
{
  private const string EndPointId = "ENP-181";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateDefaultAccessRightRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Default Access Right End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new default access right";
      s.Description = "This endpoint is used to create a new  default access right. Here details of default access right to be created is provided";
      s.ExampleRequest = new CreateDefaultAccessRightRequest { Name = "Name", Narration = "Narration", RightID = "1000", Rights = "Rights", Type = "Type" };
      s.ResponseExamples[200] = new CreateDefaultAccessRightResponse("Name", "Narration", "1000", "Rights", "Type", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateDefaultAccessRightRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<DefaultAccessRightDTO>();
    requestDTO.Id = request.RightID;

    var result = await mediator.Send(new CreateModelCommand<DefaultAccessRightDTO, DefaultAccessRight>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is DefaultAccessRightDTO obj)
      {
        Response = new CreateDefaultAccessRightResponse(obj.Name, obj.Narration, obj.Id, obj.Rights, obj.Type, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
