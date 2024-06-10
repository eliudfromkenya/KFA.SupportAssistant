using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAssignments;

public class CreateComputerAssetAssignmentRequest
{
  public const string Route = "/computer_asset_assignments";
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
