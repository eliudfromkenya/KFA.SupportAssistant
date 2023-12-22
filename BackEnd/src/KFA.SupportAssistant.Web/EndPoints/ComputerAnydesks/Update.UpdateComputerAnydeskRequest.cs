using System.ComponentModel.DataAnnotations;
using KFA.SupportAssistant.Core.DataLayer.Types;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAnydesks;

public record UpdateComputerAnydeskRequest
{
  public const string Route = "/computer_anydesks/{anyDeskId}";
  [Required]
  public string? AnyDeskId { get; set; }
  [Required]
  public string? AnyDeskNumber { get; set; }
  [Required]
  public string? CostCentreCode { get; set; }
  [Required]
  public string? DeviceName { get; set; }
  [Required]
  public string? NameOfUser { get; set; }
  public string? Narration { get; set; }
  [Required]
  public string? Password { get; set; }
  public AnyDeskComputerType? Type { get; set; }
}
