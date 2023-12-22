using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Classes;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Patch;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.PayrollGroups;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchPayrollGroupRequest, PayrollGroupRecord>
{
  private const string EndPointId = "ENP-1M6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchPayrollGroupRequest.Route));
    //RequestBinder(new PatchBinder<PayrollGroupDTO, PayrollGroup, PatchPayrollGroupRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Payroll Group End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a payroll group";
      s.Description = "Used to update part of an existing payroll group. A valid existing payroll group is required.";
      s.ResponseExamples[200] = new PayrollGroupRecord("1000", "Group Name", "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchPayrollGroupRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.GroupID))
    {
      AddError(request => request.GroupID, "The payroll group of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    PayrollGroupDTO patchFunc(PayrollGroupDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<PayrollGroupDTO, PayrollGroup>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<PayrollGroupDTO, PayrollGroup>(CreateEndPointUser.GetEndPointUser(User), request.GroupID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the payroll group to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new PayrollGroupRecord(obj.Id, obj.GroupName, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
