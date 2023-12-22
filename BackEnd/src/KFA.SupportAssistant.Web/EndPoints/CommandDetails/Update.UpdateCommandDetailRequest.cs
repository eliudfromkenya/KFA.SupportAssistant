using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.CommandDetails;

public record UpdateCommandDetailRequest
{
  public const string Route = "/command_details/{commandId}";
  [Required]
  public string? Action { get; set; }
  [Required]
  public string? ActiveState { get; set; }
  [Required]
  public string? Category { get; set; }
  [Required]
  public string? CommandId { get; set; }
  [Required]
  public string? CommandName { get; set; }
  [Required]
  public string? CommandText { get; set; }
  [Required]
  public string? ImageId { get; set; }
  [Required]
  public string? ImagePath { get; set; }
  [Required]
  public bool? IsEnabled { get; set; }
  [Required]
  public bool? IsPublished { get; set; }
  public string? Narration { get; set; }
  [Required]
  public string? ShortcutKey { get; set; }
}
