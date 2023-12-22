using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.EmployeeDetails;

/// <summary>
/// Get a employee detail by employee id.
/// </summary>
/// <remarks>
/// Takes employee id and returns a matching employee detail record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetEmployeeDetailByIdRequest, EmployeeDetailRecord>
{
  private const string EndPointId = "ENP-1B4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetEmployeeDetailByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Employee Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets employee detail by specified employee id";
      s.Description = "This endpoint is used to retrieve employee detail with the provided employee id";
      s.ExampleRequest = new GetEmployeeDetailByIdRequest { EmployeeID = "employee id to retrieve" };
      s.ResponseExamples[200] = new EmployeeDetailRecord(0, "Classification", "Cost Centre Code", DateTime.Now, "Email", "1000", "Full Name", "Gender", "Group Number", "Id Number", "Narration", string.Empty, "Payroll Number", "Phone Number", DateTime.Now, "Remarks", 0, 0, DateTime.Now, "Status", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetEmployeeDetailByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.EmployeeID))
    {
      AddError(request => request.EmployeeID, "The employee id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<EmployeeDetailDTO, EmployeeDetail>(CreateEndPointUser.GetEndPointUser(User), request.EmployeeID ?? "");
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
      Response = new EmployeeDetailRecord(obj.AmountDue, obj.Classification, obj.CostCentreCode, obj.Date, obj.Email, obj.Id, obj.FullName, obj.Gender, obj.GroupNumber, obj.IdNumber, obj.Narration, obj.PayrollGroupID, obj.PayrollNumber, obj.PhoneNumber, obj.RejoinDate, obj.Remarks, obj.RetireeAmount, obj.RetrenchmentAmount, obj.RetrenchmentDate, obj.Status, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
