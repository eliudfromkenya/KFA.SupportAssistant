using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.PayrollGroups;

public class PatchPayrollGroupRequest : JsonPatchDocument<PayrollGroupDTO>, IPlainTextRequest
{
  public const string Route = "/payroll_groups/{groupID}";

  public static string BuildRoute(string groupID) => Route.Replace("{groupID}", groupID);

  public string GroupID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<PayrollGroupDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<PayrollGroupDTO>>(Content)!;
}
