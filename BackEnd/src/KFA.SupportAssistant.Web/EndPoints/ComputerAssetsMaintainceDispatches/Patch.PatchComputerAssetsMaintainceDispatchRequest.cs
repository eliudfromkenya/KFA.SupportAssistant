using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceDispatches;

public class PatchComputerAssetsMaintainceDispatchRequest : JsonPatchDocument<ComputerAssetsMaintainceDispatchDTO>, IPlainTextRequest
{
  public const string Route = "/computer_assets_maintaince_dispatches/{dispatchID}";

  public static string BuildRoute(string dispatchID) => Route.Replace("{dispatchID}", dispatchID);

  public string DispatchID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ComputerAssetsMaintainceDispatchDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ComputerAssetsMaintainceDispatchDTO>>(Content)!;
}
