using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_computer_asset_acquisitions")]
public sealed record class ComputerAssetAcquisition : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_computer_asset_acquisitions";
  [Required]
  [Column("acquisition_id")]
  public override string? Id { get; init; }

  [MaxLength(255, ErrorMessage = "Please aquisition type must be 255 characters or less")]
  [Column("aquisition_type")]
  public string? AquisitionType { get; init; }

  [Column("asset_detail_id")]
  public string? AssetDetailID { get; init; }

  [ForeignKey(nameof(AssetDetailID))]
  public ComputerAssetDetail? AssetDetail { get; set; }
  //[Reactive, NotMapped]
  public string? AssetDetail_Caption { get; set; }

  [Column("asset_value")]
  public decimal AssetValue { get; init; }

  [Column("date_of_acquisition")]
  public global::System.DateTime DateOfAcquisition { get; init; }

  [MaxLength(255, ErrorMessage = "Please document no must be 255 characters or less")]
  [Column("document_no")]
  public string? DocumentNo { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [MaxLength(255, ErrorMessage = "Please quotation number must be 255 characters or less")]
  [Column("quotation_number")]
  public string? QuotationNumber { get; init; }

  [MaxLength(255, ErrorMessage = "Please received by must be 255 characters or less")]
  [Column("received_by")]
  public string? ReceivedBy { get; init; }

  [Column("value")]
  public decimal Value { get; init; }

  [Column("vat_amount")]
  public decimal VATAmount { get; init; }

  [Column("vendor_code")]
  public string? VendorCode { get; init; }

  [ForeignKey(nameof(VendorCode))]
  public Vendor? Vendor { get; set; }
  //[Reactive, NotMapped]
  public string? Vendor_Caption { get; set; }
  [Column("waranty_end_date")]
  public global::System.DateTime WarantyEndDate { get; init; }

  public override object ToBaseDTO()
  {
    return (ComputerAssetAcquisitionDTO)this;
  }
}
