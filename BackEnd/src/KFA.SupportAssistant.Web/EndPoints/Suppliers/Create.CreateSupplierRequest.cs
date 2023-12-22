using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.Suppliers;

public class CreateSupplierRequest
{
  public const string Route = "/suppliers";
  public string? Address { get; set; }
  public string? ContactPerson { get; set; }
  public string? CostCentreCode { get; set; }

  [Required]
  public string? Description { get; set; }

  public string? Email { get; set; }
  public string? Narration { get; set; }
  public string? PostalCode { get; set; }

  [Required]
  public string? SupplierCode { get; set; }

  [Required]
  public string? SupplierId { get; set; }

  public string? Telephone { get; set; }
  public string? Town { get; set; }
}
