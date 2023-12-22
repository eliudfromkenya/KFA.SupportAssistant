namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetDetails;

public class GetPurchasesBudgetDetailByIdRequest
{
  public const string Route = "/purchases_budget_details/{purchasesBudgetDetailId}";

  public static string BuildRoute(string? purchasesBudgetDetailId) => Route.Replace("{purchasesBudgetDetailId}", purchasesBudgetDetailId);

  public string? PurchasesBudgetDetailId { get; set; }
}
