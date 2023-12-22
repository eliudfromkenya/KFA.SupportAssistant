using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.PayrollGroups;

/// <summary>
/// Get a payroll group by group id.
/// </summary>
/// <remarks>
/// Takes group id and returns a matching payroll group record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetPayrollGroupByIdRequest, PayrollGroupRecord>
{
  private const string EndPointId = "ENP-1M4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetPayrollGroupByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Payroll Group End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets payroll group by specified group id";
      s.Description = "This endpoint is used to retrieve payroll group with the provided group id";
      s.ExampleRequest = new GetPayrollGroupByIdRequest { GroupID = "group id to retrieve" };
      s.ResponseExamples[200] = new PayrollGroupRecord("1000", "Group Name", "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetPayrollGroupByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.GroupID))
    {
      AddError(request => request.GroupID, "The group id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<PayrollGroupDTO, PayrollGroup>(CreateEndPointUser.GetEndPointUser(User), request.GroupID ?? "");
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
      Response = new PayrollGroupRecord(obj.Id, obj.GroupName, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
