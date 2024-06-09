using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsWriteOffs;

public class PatchComputerAssetsWriteOffRequest : JsonPatchDocument<ComputerAssetsWriteOffDTO>, IPlainTextRequest
{
  public const string Route = "/computer_assets_write_offs/{writeOffID}";

  public static string BuildRoute(string writeOffID) => Route.Replace("{writeOffID}", writeOffID);

  public string WriteOffID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ComputerAssetsWriteOffDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ComputerAssetsWriteOffDTO>>(Content)!;
}
