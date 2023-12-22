namespace KFA.SupportAssistant.Web.EndPoints.PayrollGroups;

public class GetPayrollGroupByIdRequest
{
  public const string Route = "/payroll_groups/{groupID}";

  public static string BuildRoute(string? groupID) => Route.Replace("{groupID}", groupID);

  public string? GroupID { get; set; }
}
