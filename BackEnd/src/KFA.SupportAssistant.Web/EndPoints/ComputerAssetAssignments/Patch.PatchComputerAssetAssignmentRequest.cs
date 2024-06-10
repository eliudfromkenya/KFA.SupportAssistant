using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAssignments;

public class PatchComputerAssetAssignmentRequest : JsonPatchDocument<ComputerAssetAssignmentDTO>, IPlainTextRequest
{
  public const string Route = "/computer_asset_assignments/{assignmentID}";

  public static string BuildRoute(string assignmentID) => Route.Replace("{assignmentID}", assignmentID);

  public string AssignmentID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ComputerAssetAssignmentDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ComputerAssetAssignmentDTO>>(Content)!;
}
