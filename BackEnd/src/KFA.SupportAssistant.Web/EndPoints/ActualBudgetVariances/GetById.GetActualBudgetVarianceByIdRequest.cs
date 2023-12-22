namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariances;

public class GetActualBudgetVarianceByIdRequest
{
  public const string Route = "/actual_budget_variances/{actualBudgetID}";

  public static string BuildRoute(string? actualBudgetID) => Route.Replace("{actualBudgetID}", actualBudgetID);

  public string? ActualBudgetID { get; set; }
}
