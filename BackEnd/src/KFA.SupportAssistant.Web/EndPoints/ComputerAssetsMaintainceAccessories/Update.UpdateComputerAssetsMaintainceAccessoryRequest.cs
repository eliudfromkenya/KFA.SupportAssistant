using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceAccessories;

public record UpdateComputerAssetsMaintainceAccessoryRequest
{
  public const string Route = "/computer_assets_maintaince_accessories/{accessoryID}";
  [Required]
  public string? AccessoryGroupID { get; set; }
  [Required]
  public string? AccessoryID { get; set; }
  public decimal? Amount { get; set; }
  [Required]
  public string? AssetDetailID { get; set; }
  public global::System.DateTime? DateOfAcquisition { get; set; }
  [Required]
  public string? Description { get; set; }
  public string? InvoiceNumber { get; set; }
  public string? MaintainceID { get; set; }
  public string? Narration { get; set; }
  public string? QuotationNumber { get; set; }
  public decimal? VATAmount { get; set; }
  public string? VendorCode { get; set; }
}
