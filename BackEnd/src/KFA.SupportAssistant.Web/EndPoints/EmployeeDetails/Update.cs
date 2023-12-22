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

namespace KFA.SupportAssistant.Web.EndPoints.EmployeeDetails;

/// <summary>
/// Update an existing employee detail.
/// </summary>
/// <remarks>
/// Update an existing employee detail by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateEmployeeDetailRequest, UpdateEmployeeDetailResponse>
{
  private const string EndPointId = "ENP-1B7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateEmployeeDetailRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Employee Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Employee Detail";
      s.Description = "This endpoint is used to update  employee detail, making a full replacement of employee detail with a specifed valuse. A valid employee detail is required.";
      s.ExampleRequest = new UpdateEmployeeDetailRequest { AmountDue = 0, Classification = "Classification", CostCentreCode = "Cost Centre Code", Date = DateTime.Now, Email = "Email", EmployeeID = "1000", FullName = "Full Name", Gender = "Gender", GroupNumber = "Group Number", IdNumber = "Id Number", Narration = "Narration", PayrollGroupID = string.Empty, PayrollNumber = "Payroll Number", PhoneNumber = "Phone Number", RejoinDate = DateTime.Now, Remarks = "Remarks", RetireeAmount = 0, RetrenchmentAmount = 0, RetrenchmentDate = DateTime.Now, Status = "Status" };
      s.ResponseExamples[200] = new UpdateEmployeeDetailResponse(new EmployeeDetailRecord(0, "Classification", "Cost Centre Code", DateTime.Now, "Email", "1000", "Full Name", "Gender", "Group Number", "Id Number", "Narration", string.Empty, "Payroll Number", "Phone Number", DateTime.Now, "Remarks", 0, 0, DateTime.Now, "Status", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateEmployeeDetailRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.EmployeeID))
    {
      AddError(request => request.EmployeeID, "The employee id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<EmployeeDetailDTO, EmployeeDetail>(CreateEndPointUser.GetEndPointUser(User), request.EmployeeID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the employee detail to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<EmployeeDetailDTO, EmployeeDetail>(CreateEndPointUser.GetEndPointUser(User), request.EmployeeID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateEmployeeDetailResponse(new EmployeeDetailRecord(obj.AmountDue, obj.Classification, obj.CostCentreCode, obj.Date, obj.Email, obj.Id, obj.FullName, obj.Gender, obj.GroupNumber, obj.IdNumber, obj.Narration, obj.PayrollGroupID, obj.PayrollNumber, obj.PhoneNumber, obj.RejoinDate, obj.Remarks, obj.RetireeAmount, obj.RetrenchmentAmount, obj.RetrenchmentDate, obj.Status, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
