using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.StaffGroups;

public class PatchStaffGroupRequest : JsonPatchDocument<StaffGroupDTO>, IPlainTextRequest
{
  public const string Route = "/staff_groups/{groupNumber}";

  public static string BuildRoute(string groupNumber) => Route.Replace("{groupNumber}", groupNumber);

  public string GroupNumber { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<StaffGroupDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<StaffGroupDTO>>(Content)!;
}
