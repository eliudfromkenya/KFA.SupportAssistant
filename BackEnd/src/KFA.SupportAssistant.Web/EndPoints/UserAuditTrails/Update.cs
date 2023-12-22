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

namespace KFA.SupportAssistant.Web.EndPoints.UserAuditTrails;

/// <summary>
/// Update an existing user audit trail.
/// </summary>
/// <remarks>
/// Update an existing user audit trail by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateUserAuditTrailRequest, UpdateUserAuditTrailResponse>
{
  private const string EndPointId = "ENP-237";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateUserAuditTrailRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update User Audit Trail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full User Audit Trail";
      s.Description = "This endpoint is used to update  user audit trail, making a full replacement of user audit trail with a specifed valuse. A valid user audit trail is required.";
      s.ExampleRequest = new UpdateUserAuditTrailRequest { ActivityDate = DateTime.Now, ActivityEnumNumber = 0, AuditId = "1000", Category = "Category", CommandId = string.Empty, Data = "Data", Description = "Description", LoginId = string.Empty, Narration = "Narration", OldValues = "Old Values" };
      s.ResponseExamples[200] = new UpdateUserAuditTrailResponse(new UserAuditTrailRecord(DateTime.Now, 0, "1000", "Category", string.Empty, "Data", "Description", string.Empty, "Narration", "Old Values", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateUserAuditTrailRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AuditId))
    {
      AddError(request => request.AuditId, "The audit id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<UserAuditTrailDTO, UserAuditTrail>(CreateEndPointUser.GetEndPointUser(User), request.AuditId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the user audit trail to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<UserAuditTrailDTO, UserAuditTrail>(CreateEndPointUser.GetEndPointUser(User), request.AuditId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateUserAuditTrailResponse(new UserAuditTrailRecord(obj.ActivityDate, obj.ActivityEnumNumber, obj.Id, obj.Category, obj.CommandId, obj.Data, obj.Description, obj.LoginId, obj.Narration, obj.OldValues, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
