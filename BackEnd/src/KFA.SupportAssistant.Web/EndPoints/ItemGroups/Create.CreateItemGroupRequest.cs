using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ItemGroups;

public class CreateItemGroupRequest
{
  public const string Route = "/item_groups";

  [Required]
  public string? GroupId { get; set; }

  [Required]
  public string? Name { get; set; }

  public string? ParentGroupId { get; set; }
}
