namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetBatchHeaders;

public record DeletePurchasesBudgetBatchHeaderRequest
{
  public const string Route = "/purchases_budget_batch_headers/{batchKey}";
  public static string BuildRoute(string? batchKey) => Route.Replace("{batchKey}", batchKey);
  public string? BatchKey { get; set; }
}
