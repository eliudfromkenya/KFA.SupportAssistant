namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetDetails;

public record DeletePurchasesBudgetDetailRequest
{
  public const string Route = "/purchases_budget_details/{purchasesBudgetDetailId}";
  public static string BuildRoute(string? purchasesBudgetDetailId) => Route.Replace("{purchasesBudgetDetailId}", purchasesBudgetDetailId);
  public string? PurchasesBudgetDetailId { get; set; }
}
