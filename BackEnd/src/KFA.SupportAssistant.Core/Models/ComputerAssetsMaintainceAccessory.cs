using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_computer_assets_maintaince_accessories")]
public sealed record class ComputerAssetsMaintainceAccessory : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_computer_assets_maintaince_accessories";
  [Required]
  [Column("accessory_group_id")]
  public string? AccessoryGroupID { get; init; }

  [ForeignKey(nameof(AccessoryGroupID))]
  public ComputerAssetGroup? AccessoryGroup { get; set; }
  //[Reactive, NotMapped]
  public string? AccessoryGroup_Caption { get; set; }

  [Required]
  [Column("accessory_id")]
  public override string? Id { get; init; }

  [Column("amount")]
  public decimal Amount { get; init; }

  [Required]
  [Column("asset_detail_id")]
  public string? AssetDetailID { get; init; }

  [ForeignKey(nameof(AssetDetailID))]
  public ComputerAssetDetail? AssetDetail { get; set; }
  //[Reactive, NotMapped]
  public string? AssetDetail_Caption { get; set; }

  [Column("date_of_acquisition")]
  public global::System.DateTime DateOfAcquisition { get; init; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [MaxLength(255, ErrorMessage = "Please invoice number must be 255 characters or less")]
  [Column("invoice_number")]
  public string? InvoiceNumber { get; init; }

  [Column("maintaince_id")]
  public string? MaintainceID { get; init; }

  [ForeignKey(nameof(MaintainceID))]
  public ComputerAssetMaintaince? Maintaince { get; set; }
  //[Reactive, NotMapped]
  public string? Maintaince_Caption { get; set; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [MaxLength(255, ErrorMessage = "Please quotation number must be 255 characters or less")]
  [Column("quotation_number")]
  public string? QuotationNumber { get; init; }

  [Column("vat_amount")]
  public decimal VATAmount { get; init; }

  [Column("vendor_code")]
  public string? VendorCode { get; init; }

  [ForeignKey(nameof(VendorCode))]
  public Vendor? Vendor { get; set; }
  //[Reactive, NotMapped]
  public string? Vendor_Caption { get; set; }
  public override object ToBaseDTO()
  {
    return (ComputerAssetsMaintainceAccessoryDTO)this;
  }
}
