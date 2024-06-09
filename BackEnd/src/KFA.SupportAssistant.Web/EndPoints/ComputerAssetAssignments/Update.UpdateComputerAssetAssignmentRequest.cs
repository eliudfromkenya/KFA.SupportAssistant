using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAssignments;

public record UpdateComputerAssetAssignmentRequest
{
  public const string Route = "/computer_asset_assignments/{assignmentID}";
  public string? AssetID { get; set; }
  public string? AssignedUser { get; set; }
  public global::System.DateTime? AssignmentDate { get; set; }
  public string? AssignmentType { get; set; }
  [Required]
  public string? AssignmentID { get; set; }
  public string? CostCentreCode { get; set; }
  public string? Narration { get; set; }
  public string? PayrollNumber { get; set; }
}
