using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerRemoteAddresses;

public record UpdateComputerRemoteAddressRequest
{
  public const string Route = "/computer_remote_addresses/{anyDeskId}";
  [Required]
  public string? AnyDeskId { get; set; }
  [Required]
  public string? AnyDeskNumber { get; set; }
  [Required]
  public string? AnydeskPassword { get; set; }
  public string? AssetDetailID { get; set; }
  [Required]
  public string? CostCentreCode { get; set; }
  [Required]
  public string? DeviceName { get; set; }
  [Required]
  public string? NameOfUser { get; set; }
  public string? Narration { get; set; }
  public string? TeamViewerAddress { get; set; }
  public string? Type { get; set; }
}
