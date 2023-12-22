using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAnydesks;

public class PatchComputerAnydeskRequest : JsonPatchDocument<ComputerAnydeskDTO>, IPlainTextRequest
{
  public const string Route = "/computer_anydesks/{anyDeskId}";

  public static string BuildRoute(string anyDeskId) => Route.Replace("{anyDeskId}", anyDeskId);

  public string AnyDeskId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ComputerAnydeskDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ComputerAnydeskDTO>>(Content)!;
}
