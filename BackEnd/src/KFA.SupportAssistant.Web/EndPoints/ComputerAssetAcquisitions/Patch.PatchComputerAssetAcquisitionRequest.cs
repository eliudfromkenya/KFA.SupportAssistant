using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAcquisitions;

public class PatchComputerAssetAcquisitionRequest : JsonPatchDocument<ComputerAssetAcquisitionDTO>, IPlainTextRequest
{
  public const string Route = "/computer_asset_acquisitions/{acquisitionID}";

  public static string BuildRoute(string acquisitionID) => Route.Replace("{acquisitionID}", acquisitionID);

  public string AcquisitionID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ComputerAssetAcquisitionDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ComputerAssetAcquisitionDTO>>(Content)!;
}
