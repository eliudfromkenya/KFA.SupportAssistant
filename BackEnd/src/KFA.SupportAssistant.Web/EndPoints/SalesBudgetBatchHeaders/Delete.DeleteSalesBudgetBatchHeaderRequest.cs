namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetBatchHeaders;

public record DeleteSalesBudgetBatchHeaderRequest
{
  public const string Route = "/sales_budget_batch_headers/{batchKey}";
  public static string BuildRoute(string? batchKey) => Route.Replace("{batchKey}", batchKey);
  public string? BatchKey { get; set; }
}
