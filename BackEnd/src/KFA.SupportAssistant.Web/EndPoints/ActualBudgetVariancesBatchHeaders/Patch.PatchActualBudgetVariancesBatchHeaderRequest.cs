using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariancesBatchHeaders;

public class PatchActualBudgetVariancesBatchHeaderRequest : JsonPatchDocument<ActualBudgetVariancesBatchHeaderDTO>, IPlainTextRequest
{
  public const string Route = "/actual_budget_variances_batch_headers/{batchKey}";

  public static string BuildRoute(string batchKey) => Route.Replace("{batchKey}", batchKey);

  public string BatchKey { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ActualBudgetVariancesBatchHeaderDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ActualBudgetVariancesBatchHeaderDTO>>(Content)!;
}
