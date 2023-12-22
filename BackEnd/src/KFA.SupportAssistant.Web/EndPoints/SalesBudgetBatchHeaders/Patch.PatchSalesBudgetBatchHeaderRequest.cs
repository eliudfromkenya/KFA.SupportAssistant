using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetBatchHeaders;

public class PatchSalesBudgetBatchHeaderRequest : JsonPatchDocument<SalesBudgetBatchHeaderDTO>, IPlainTextRequest
{
  public const string Route = "/sales_budget_batch_headers/{batchKey}";

  public static string BuildRoute(string batchKey) => Route.Replace("{batchKey}", batchKey);

  public string BatchKey { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<SalesBudgetBatchHeaderDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<SalesBudgetBatchHeaderDTO>>(Content)!;
}
