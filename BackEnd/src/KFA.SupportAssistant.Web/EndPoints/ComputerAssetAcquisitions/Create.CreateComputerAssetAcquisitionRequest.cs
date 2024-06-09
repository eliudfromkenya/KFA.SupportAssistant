using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAcquisitions;

public class CreateComputerAssetAcquisitionRequest
{
  public const string Route = "/computer_asset_acquisitions";
  [Required]
  public string? AcquisitionID { get; set; }
  public string? AquisitionType { get; set; }
  public string? AssetDetailID { get; set; }
  public decimal? AssetValue { get; set; }
  public global::System.DateTime? DateOfAcquisition { get; set; }
  public string? DocumentNo { get; set; }
  public string? Narration { get; set; }
  public string? QuotationNumber { get; set; }
  public string? ReceivedBy { get; set; }
  public decimal? Value { get; set; }
  public decimal? VATAmount { get; set; }
  public string? VendorCode { get; set; }
  public global::System.DateTime? WarantyEndDate { get; set; }
}
