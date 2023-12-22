namespace KFA.SupportAssistant.Web.EndPoints.EmployeeDetails;

public class GetEmployeeDetailByIdRequest
{
  public const string Route = "/employee_details/{employeeID}";

  public static string BuildRoute(string? employeeID) => Route.Replace("{employeeID}", employeeID);

  public string? EmployeeID { get; set; }
}
