using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.EmployeeDetails;

/// <summary>
/// Create a new EmployeeDetail
/// </summary>
/// <remarks>
/// Creates a new employee detail given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateEmployeeDetailRequest, CreateEmployeeDetailResponse>
{
  private const string EndPointId = "ENP-1B1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateEmployeeDetailRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Employee Detail End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new employee detail";
      s.Description = "This endpoint is used to create a new  employee detail. Here details of employee detail to be created is provided";
      s.ExampleRequest = new CreateEmployeeDetailRequest { AmountDue = 0, Classification = "Classification", CostCentreCode = "Cost Centre Code", Date = DateTime.Now, Email = "Email", EmployeeID = "1000", FullName = "Full Name", Gender = "Gender", GroupNumber = "Group Number", IdNumber = "Id Number", Narration = "Narration", PayrollGroupID = string.Empty, PayrollNumber = "Payroll Number", PhoneNumber = "Phone Number", RejoinDate = DateTime.Now, Remarks = "Remarks", RetireeAmount = 0, RetrenchmentAmount = 0, RetrenchmentDate = DateTime.Now, Status = "Status" };
      s.ResponseExamples[200] = new CreateEmployeeDetailResponse(0, "Classification", "Cost Centre Code", DateTime.Now, "Email", "1000", "Full Name", "Gender", "Group Number", "Id Number", "Narration", string.Empty, "Payroll Number", "Phone Number", DateTime.Now, "Remarks", 0, 0, DateTime.Now, "Status", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateEmployeeDetailRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<EmployeeDetailDTO>();
    requestDTO.Id = request.EmployeeID;

    var result = await mediator.Send(new CreateModelCommand<EmployeeDetailDTO, EmployeeDetail>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is EmployeeDetailDTO obj)
      {
        Response = new CreateEmployeeDetailResponse(obj.AmountDue, obj.Classification, obj.CostCentreCode, obj.Date, obj.Email, obj.Id, obj.FullName, obj.Gender, obj.GroupNumber, obj.IdNumber, obj.Narration, obj.PayrollGroupID, obj.PayrollNumber, obj.PhoneNumber, obj.RejoinDate, obj.Remarks, obj.RetireeAmount, obj.RetrenchmentAmount, obj.RetrenchmentDate, obj.Status, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
