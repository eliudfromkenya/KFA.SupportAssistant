using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.Vendors;

public class CreateVendorRequest
{
  public const string Route = "/vendors";
  public string? Contact { get; set; }
  [Required]
  public string? Descriptions { get; set; }
  public string? Email { get; set; }
  [Required]
  public bool? IsActive { get; set; }
  [Required]
  public string? VendorCode { get; set; }
}
