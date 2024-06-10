using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetValuations;

public class PatchComputerAssetValuationRequest : JsonPatchDocument<ComputerAssetValuationDTO>, IPlainTextRequest
{
  public const string Route = "/computer_asset_valuations/{revaluationID}";

  public static string BuildRoute(string revaluationID) => Route.Replace("{revaluationID}", revaluationID);

  public string RevaluationID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ComputerAssetValuationDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ComputerAssetValuationDTO>>(Content)!;
}
