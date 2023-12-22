namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariancesBatchHeaders;

public record DeleteActualBudgetVariancesBatchHeaderRequest
{
  public const string Route = "/actual_budget_variances_batch_headers/{batchKey}";
  public static string BuildRoute(string? batchKey) => Route.Replace("{batchKey}", batchKey);
  public string? BatchKey { get; set; }
}
