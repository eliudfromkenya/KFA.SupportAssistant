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

namespace KFA.SupportAssistant.Web.EndPoints.PayrollGroups;

/// <summary>
/// Update an existing payroll group.
/// </summary>
/// <remarks>
/// Update an existing payroll group by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdatePayrollGroupRequest, UpdatePayrollGroupResponse>
{
  private const string EndPointId = "ENP-1M7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdatePayrollGroupRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Payroll Group End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Payroll Group";
      s.Description = "This endpoint is used to update  payroll group, making a full replacement of payroll group with a specifed valuse. A valid payroll group is required.";
      s.ExampleRequest = new UpdatePayrollGroupRequest { GroupID = "1000", GroupName = "Group Name", Narration = "Narration" };
      s.ResponseExamples[200] = new UpdatePayrollGroupResponse(new PayrollGroupRecord("1000", "Group Name", "Narration", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdatePayrollGroupRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.GroupID))
    {
      AddError(request => request.GroupID, "The group id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<PayrollGroupDTO, PayrollGroup>(CreateEndPointUser.GetEndPointUser(User), request.GroupID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the payroll group to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<PayrollGroupDTO, PayrollGroup>(CreateEndPointUser.GetEndPointUser(User), request.GroupID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdatePayrollGroupResponse(new PayrollGroupRecord(obj.Id, obj.GroupName, obj.Narration, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
