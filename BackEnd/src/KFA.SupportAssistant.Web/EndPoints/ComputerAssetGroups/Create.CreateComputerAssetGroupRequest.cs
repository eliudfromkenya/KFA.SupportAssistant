using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetGroups;

public class CreateComputerAssetGroupRequest
{
  public const string Route = "/computer_asset_groups";
  [Required]
  public bool? CanBeAssigned { get; set; }
  public string? Description { get; set; }
  [Required]
  public string? GroupID { get; set; }
  [Required]
  public string? GroupName { get; set; }
  public string? LastAssignedValue { get; set; }
  public string? ParentGroupID { get; set; }
  public string? Prefix { get; set; }
  public string? Suffix { get; set; }
}
