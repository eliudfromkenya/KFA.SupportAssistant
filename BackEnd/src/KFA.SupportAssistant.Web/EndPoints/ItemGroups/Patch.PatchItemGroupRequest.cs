using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ItemGroups;

public class PatchItemGroupRequest : JsonPatchDocument<ItemGroupDTO>, IPlainTextRequest
{
  public const string Route = "/item_groups/{groupId}";

  public static string BuildRoute(string groupId) => Route.Replace("{groupId}", groupId);

  public string GroupId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ItemGroupDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ItemGroupDTO>>(Content)!;
}
