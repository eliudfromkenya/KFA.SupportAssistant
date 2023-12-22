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

namespace KFA.SupportAssistant.Web.EndPoints.EmployeeDetails;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchEmployeeDetailRequest, EmployeeDetailRecord>
{
  private const string EndPointId = "ENP-1B6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchEmployeeDetailRequest.Route));
    //RequestBinder(new PatchBinder<EmployeeDetailDTO, EmployeeDetail, PatchEmployeeDetailRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Employee Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a employee detail";
      s.Description = "Used to update part of an existing employee detail. A valid existing employee detail is required.";
      s.ResponseExamples[200] = new EmployeeDetailRecord(0, "Classification", "Cost Centre Code", DateTime.Now, "Email", "1000", "Full Name", "Gender", "Group Number", "Id Number", "Narration", string.Empty, "Payroll Number", "Phone Number", DateTime.Now, "Remarks", 0, 0, DateTime.Now, "Status", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchEmployeeDetailRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.EmployeeID))
    {
      AddError(request => request.EmployeeID, "The employee detail of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    EmployeeDetailDTO patchFunc(EmployeeDetailDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<EmployeeDetailDTO, EmployeeDetail>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<EmployeeDetailDTO, EmployeeDetail>(CreateEndPointUser.GetEndPointUser(User), request.EmployeeID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the employee detail to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new EmployeeDetailRecord(obj.AmountDue, obj.Classification, obj.CostCentreCode, obj.Date, obj.Email, obj.Id, obj.FullName, obj.Gender, obj.GroupNumber, obj.IdNumber, obj.Narration, obj.PayrollGroupID, obj.PayrollNumber, obj.PhoneNumber, obj.RejoinDate, obj.Remarks, obj.RetireeAmount, obj.RetrenchmentAmount, obj.RetrenchmentDate, obj.Status, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
