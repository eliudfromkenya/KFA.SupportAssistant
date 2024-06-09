using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ComputerAssetAcquisitionDTO : BaseDTO<ComputerAssetAcquisition>
{
  public string? AquisitionType { get; set; }
  public string? AssetDetailID { get; set; }
  public decimal AssetValue { get; set; }
  public global::System.DateTime DateOfAcquisition { get; set; }
  public string? DocumentNo { get; set; }
  public string? Narration { get; set; }
  public string? QuotationNumber { get; set; }
  public string? ReceivedBy { get; set; }
  public decimal Value { get; set; }
  public decimal VATAmount { get; set; }
  public string? VendorCode { get; set; }
  public global::System.DateTime WarantyEndDate { get; set; }
  public override ComputerAssetAcquisition? ToModel()
  {
    return (ComputerAssetAcquisition)this;
  }

  public static implicit operator ComputerAssetAcquisitionDTO(ComputerAssetAcquisition obj)
  {
    return new ComputerAssetAcquisitionDTO
    {
      AquisitionType = obj.AquisitionType,
      AssetDetailID = obj.AssetDetailID,
      AssetValue = obj.AssetValue,
      DateOfAcquisition = obj.DateOfAcquisition,
      DocumentNo = obj.DocumentNo,
      Narration = obj.Narration,
      QuotationNumber = obj.QuotationNumber,
      ReceivedBy = obj.ReceivedBy,
      Value = obj.Value,
      VATAmount = obj.VATAmount,
      VendorCode = obj.VendorCode,
      WarantyEndDate = obj.WarantyEndDate,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ComputerAssetAcquisition(ComputerAssetAcquisitionDTO obj)
  {
    return new ComputerAssetAcquisition
    {
      AquisitionType = obj.AquisitionType,
      AssetDetailID = obj.AssetDetailID,
      AssetValue = obj.AssetValue,
      DateOfAcquisition = obj.DateOfAcquisition,
      DocumentNo = obj.DocumentNo,
      Narration = obj.Narration,
      QuotationNumber = obj.QuotationNumber,
      ReceivedBy = obj.ReceivedBy,
      Value = obj.Value,
      VATAmount = obj.VATAmount,
      VendorCode = obj.VendorCode,
      WarantyEndDate = obj.WarantyEndDate,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
