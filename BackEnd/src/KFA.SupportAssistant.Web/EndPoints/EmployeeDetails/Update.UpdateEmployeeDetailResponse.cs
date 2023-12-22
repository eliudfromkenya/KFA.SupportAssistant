namespace KFA.SupportAssistant.Web.EndPoints.EmployeeDetails;

public class UpdateEmployeeDetailResponse
{
  public UpdateEmployeeDetailResponse(EmployeeDetailRecord employeeDetail)
  {
    EmployeeDetail = employeeDetail;
  }

  public EmployeeDetailRecord EmployeeDetail { get; set; }
}
