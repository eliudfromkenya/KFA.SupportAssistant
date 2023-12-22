using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAnydesks;

public class CreateComputerAnydeskRequest
{
  public const string Route = "/computer_anydesks";

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

  public string? Type { get; set; }
}
