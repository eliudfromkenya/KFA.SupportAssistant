using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ExpenseBudgetBatchHeaders;

public class PatchExpenseBudgetBatchHeaderRequest : JsonPatchDocument<ExpenseBudgetBatchHeaderDTO>, IPlainTextRequest
{
  public const string Route = "/expense_budget_batch_headers/{batchKey}";

  public static string BuildRoute(string batchKey) => Route.Replace("{batchKey}", batchKey);

  public string BatchKey { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ExpenseBudgetBatchHeaderDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ExpenseBudgetBatchHeaderDTO>>(Content)!;
}
