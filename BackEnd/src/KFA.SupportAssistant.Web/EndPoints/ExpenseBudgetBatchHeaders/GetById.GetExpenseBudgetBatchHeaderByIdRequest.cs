namespace KFA.SupportAssistant.Web.EndPoints.ExpenseBudgetBatchHeaders;

public class GetExpenseBudgetBatchHeaderByIdRequest
{
  public const string Route = "/expense_budget_batch_headers/{batchKey}";

  public static string BuildRoute(string? batchKey) => Route.Replace("{batchKey}", batchKey);

  public string? BatchKey { get; set; }
}
