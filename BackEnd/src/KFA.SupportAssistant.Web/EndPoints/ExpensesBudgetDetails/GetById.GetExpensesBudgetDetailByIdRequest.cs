namespace KFA.SupportAssistant.Web.EndPoints.ExpensesBudgetDetails;

public class GetExpensesBudgetDetailByIdRequest
{
  public const string Route = "/expenses_budget_details/{expenseBudgetDetailId}";

  public static string BuildRoute(string? expenseBudgetDetailId) => Route.Replace("{expenseBudgetDetailId}", expenseBudgetDetailId);

  public string? ExpenseBudgetDetailId { get; set; }
}
