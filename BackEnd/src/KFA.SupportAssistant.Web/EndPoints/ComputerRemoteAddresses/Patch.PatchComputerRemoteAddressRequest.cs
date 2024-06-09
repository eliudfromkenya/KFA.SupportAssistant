using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerRemoteAddresses;

public class PatchComputerRemoteAddressRequest : JsonPatchDocument<ComputerRemoteAddressDTO>, IPlainTextRequest
{
  public const string Route = "/computer_remote_addresses/{anyDeskId}";

  public static string BuildRoute(string anyDeskId) => Route.Replace("{anyDeskId}", anyDeskId);

  public string AnyDeskId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ComputerRemoteAddressDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ComputerRemoteAddressDTO>>(Content)!;
}
