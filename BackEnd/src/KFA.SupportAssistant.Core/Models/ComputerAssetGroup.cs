using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_computer_asset_groups")]
public sealed record class ComputerAssetGroup : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_computer_asset_groups";
  [Required]
  [Column("can_be_assigned")]
  public bool CanBeAssigned { get; init; }

  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [Required]
  [Column("group_id")]
  public override string? Id { get; init; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please group name must be 255 characters or less")]
  [Column("group_name")]
  public string? GroupName { get; init; }

  [MaxLength(255, ErrorMessage = "Please last assigned value must be 255 characters or less")]
  [Column("last_assigned_value")]
  public string? LastAssignedValue { get; init; }

  [Column("parent_group_id")]
  public string? ParentGroupID { get; init; }

  [ForeignKey(nameof(ParentGroupID))]
  public ComputerAssetGroup? ParentGroup { get; set; }
  //[Reactive, NotMapped]
  public string? ParentGroup_Caption { get; set; }

  [MaxLength(255, ErrorMessage = "Please prefix must be 255 characters or less")]
  [Column("prefix")]
  public string? Prefix { get; init; }

  [MaxLength(255, ErrorMessage = "Please suffix must be 255 characters or less")]
  [Column("suffix")]
  public string? Suffix { get; init; }

  public ICollection<ComputerAssetDetail>? ComputerAssetDetails { get; set; }
  public ICollection<ComputerAssetGroup>? ComputerAssetGroups { get; set; }
  public ICollection<ComputerAssetsMaintainceAccessory>? ComputerAssetsMaintainceAccessories { get; set; }
  public override object ToBaseDTO()
  {
    return (ComputerAssetGroupDTO)this;
  }
}
