using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ItemGroups;

public record UpdateItemGroupRequest
{
  public const string Route = "/item_groups/{groupId}";
  [Required]
  public string? GroupId { get; set; }
  [Required]
  public string? Name { get; set; }
  public string? ParentGroupId { get; set; }
}
