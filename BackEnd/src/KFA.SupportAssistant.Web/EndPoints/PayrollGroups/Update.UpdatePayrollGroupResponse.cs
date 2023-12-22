namespace KFA.SupportAssistant.Web.EndPoints.PayrollGroups;

public class UpdatePayrollGroupResponse
{
  public UpdatePayrollGroupResponse(PayrollGroupRecord payrollGroup)
  {
    PayrollGroup = payrollGroup;
  }

  public PayrollGroupRecord PayrollGroup { get; set; }
}
