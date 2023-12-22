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

namespace KFA.SupportAssistant.Web.EndPoints.VerificationRights;

/// <summary>
/// Update an existing verification right.
/// </summary>
/// <remarks>
/// Update an existing verification right by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateVerificationRightRequest, UpdateVerificationRightResponse>
{
  private const string EndPointId = "ENP-287";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateVerificationRightRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Verification Right End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Verification Right";
      s.Description = "This endpoint is used to update  verification right, making a full replacement of verification right with a specifed valuse. A valid verification right is required.";
      s.ExampleRequest = new UpdateVerificationRightRequest { DeviceId = string.Empty, UserId = string.Empty, UserRoleId = string.Empty, VerificationRightId = "1000", VerificationTypeId = string.Empty };
      s.ResponseExamples[200] = new UpdateVerificationRightResponse(new VerificationRightRecord(string.Empty, string.Empty, string.Empty, "1000", 0, DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateVerificationRightRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VerificationRightId))
    {
      AddError(request => request.VerificationRightId, "The verification right id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<VerificationRightDTO, VerificationRight>(CreateEndPointUser.GetEndPointUser(User), request.VerificationRightId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the verification right to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<VerificationRightDTO, VerificationRight>(CreateEndPointUser.GetEndPointUser(User), request.VerificationRightId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateVerificationRightResponse(new VerificationRightRecord(obj.DeviceId, obj.UserId, obj.UserRoleId, obj.Id, obj.VerificationTypeId, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
