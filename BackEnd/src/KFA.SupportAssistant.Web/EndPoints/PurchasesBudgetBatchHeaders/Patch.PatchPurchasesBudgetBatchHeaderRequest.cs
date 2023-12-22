using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetBatchHeaders;

public class PatchPurchasesBudgetBatchHeaderRequest : JsonPatchDocument<PurchasesBudgetBatchHeaderDTO>, IPlainTextRequest
{
  public const string Route = "/purchases_budget_batch_headers/{batchKey}";

  public static string BuildRoute(string batchKey) => Route.Replace("{batchKey}", batchKey);

  public string BatchKey { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<PurchasesBudgetBatchHeaderDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<PurchasesBudgetBatchHeaderDTO>>(Content)!;
}
