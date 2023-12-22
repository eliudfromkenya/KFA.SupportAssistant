using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.UseCases.Models.Update;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.SystemRights;

/// <summary>
/// Update an existing system right.
/// </summary>
/// <remarks>
/// Update an existing system right by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateSystemRightRequest, UpdateSystemRightResponse>
{
  private const string EndPointId = "ENP-207";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateSystemRightRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update System Right End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full System Right";
      s.Description = "This endpoint is used to update  system right, making a full replacement of system right with a specifed valuse. A valid system right is required.";
      s.ExampleRequest = new UpdateSystemRightRequest { IsCompulsory = true, Narration = "Narration", RightId = "1000", RightName = "Right Name" };
      s.ResponseExamples[200] = new UpdateSystemRightResponse(new SystemRightRecord(true, "Narration", "1000", "Right Name", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateSystemRightRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.RightId))
    {
      AddError(request => request.RightId, "The right id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<SystemRightDTO, SystemRight>(CreateEndPointUser.GetEndPointUser(User), request.RightId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the system right to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<SystemRightDTO, SystemRight>(CreateEndPointUser.GetEndPointUser(User), request.RightId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateSystemRightResponse(new SystemRightRecord(obj.IsCompulsory, obj.Narration, obj.Id, obj.RightName, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
