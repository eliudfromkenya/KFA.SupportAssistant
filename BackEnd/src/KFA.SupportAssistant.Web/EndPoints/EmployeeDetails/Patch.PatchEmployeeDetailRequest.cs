using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.EmployeeDetails;

public class PatchEmployeeDetailRequest : JsonPatchDocument<EmployeeDetailDTO>, IPlainTextRequest
{
  public const string Route = "/employee_details/{employeeID}";

  public static string BuildRoute(string employeeID) => Route.Replace("{employeeID}", employeeID);

  public string EmployeeID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<EmployeeDetailDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<EmployeeDetailDTO>>(Content)!;
}
