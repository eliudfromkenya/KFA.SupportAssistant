namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetDetails;

public class GetSalesBudgetDetailByIdRequest
{
  public const string Route = "/sales_budget_details/{salesBudgetDetailId}";

  public static string BuildRoute(string? salesBudgetDetailId) => Route.Replace("{salesBudgetDetailId}", salesBudgetDetailId);

  public string? SalesBudgetDetailId { get; set; }
}
