using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_computer_asset_assignments")]
public sealed record class ComputerAssetAssignment : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_computer_asset_assignments";
  [Column("asset_id")]
  public string? AssetID { get; init; }

  [ForeignKey(nameof(AssetID))]
  public ComputerAssetDetail? Asset { get; set; }
  //[Reactive, NotMapped]
  public string? Asset_Caption { get; set; }

  [MaxLength(255, ErrorMessage = "Please assigned user must be 255 characters or less")]
  [Column("assigned_user")]
  public string? AssignedUser { get; init; }

  [Column("assignment_date")]
  public global::System.DateTime AssignmentDate { get; init; }

  [MaxLength(255, ErrorMessage = "Please assignment type must be 255 characters or less")]
  [Column("assignment_type")]
  public string? AssignmentType { get; init; }

  [Required]
  [Column("assignmentid")]
  public override string? Id { get; init; }

  [Column("cost_centre_code")]
  public string? CostCentreCode { get; init; }

  [ForeignKey(nameof(CostCentreCode))]
  public CostCentre? CostCentre { get; set; }
  //[Reactive, NotMapped]
  public string? CostCentre_Caption { get; set; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [MaxLength(255, ErrorMessage = "Please payroll number must be 255 characters or less")]
  [Column("payroll_number")]
  public string? PayrollNumber { get; init; }
  public override object ToBaseDTO()
  {
    return (ComputerAssetAssignmentDTO)this;
  }
}
