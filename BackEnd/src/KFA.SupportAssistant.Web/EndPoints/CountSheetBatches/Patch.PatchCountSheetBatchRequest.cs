using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.CountSheetBatches;

public class PatchCountSheetBatchRequest : JsonPatchDocument<CountSheetBatchDTO>, IPlainTextRequest
{
  public const string Route = "/count_sheet_batches/{batchKey}";

  public static string BuildRoute(string batchKey) => Route.Replace("{batchKey}", batchKey);

  public string BatchKey { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<CountSheetBatchDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<CountSheetBatchDTO>>(Content)!;
}
