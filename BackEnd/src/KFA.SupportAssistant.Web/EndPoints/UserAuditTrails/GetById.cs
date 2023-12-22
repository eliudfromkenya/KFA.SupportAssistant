using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.UserAuditTrails;

/// <summary>
/// Get a user audit trail by audit id.
/// </summary>
/// <remarks>
/// Takes audit id and returns a matching user audit trail record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetUserAuditTrailByIdRequest, UserAuditTrailRecord>
{
  private const string EndPointId = "ENP-234";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetUserAuditTrailByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get User Audit Trail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets user audit trail by specified audit id";
      s.Description = "This endpoint is used to retrieve user audit trail with the provided audit id";
      s.ExampleRequest = new GetUserAuditTrailByIdRequest { AuditId = "audit id to retrieve" };
      s.ResponseExamples[200] = new UserAuditTrailRecord(DateTime.Now, 0, "1000", "Category", string.Empty, "Data", "Description", string.Empty, "Narration", "Old Values", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetUserAuditTrailByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AuditId))
    {
      AddError(request => request.AuditId, "The audit id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<UserAuditTrailDTO, UserAuditTrail>(CreateEndPointUser.GetEndPointUser(User), request.AuditId ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }
    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UserAuditTrailRecord(obj.ActivityDate, obj.ActivityEnumNumber, obj.Id, obj.Category, obj.CommandId, obj.Data, obj.Description, obj.LoginId, obj.Narration, obj.OldValues, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
