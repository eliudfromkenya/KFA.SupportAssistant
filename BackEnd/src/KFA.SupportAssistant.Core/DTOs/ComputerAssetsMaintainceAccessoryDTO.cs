using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ComputerAssetsMaintainceAccessoryDTO : BaseDTO<ComputerAssetsMaintainceAccessory>
{
  public string? AccessoryGroupID { get; set; }
  public decimal Amount { get; set; }
  public string? AssetDetailID { get; set; }
  public global::System.DateTime DateOfAcquisition { get; set; }
  public string? Description { get; set; }
  public string? InvoiceNumber { get; set; }
  public string? MaintainceID { get; set; }
  public string? Narration { get; set; }
  public string? QuotationNumber { get; set; }
  public decimal VATAmount { get; set; }
  public string? VendorCode { get; set; }

  public static implicit operator ComputerAssetsMaintainceAccessoryDTO(ComputerAssetsMaintainceAccessory obj)
  {
    return new ComputerAssetsMaintainceAccessoryDTO
    {
      AccessoryGroupID = obj.AccessoryGroupID,
      Amount = obj.Amount,
      AssetDetailID = obj.AssetDetailID,
      DateOfAcquisition = obj.DateOfAcquisition,
      Description = obj.Description,
      InvoiceNumber = obj.InvoiceNumber,
      MaintainceID = obj.MaintainceID,
      Narration = obj.Narration,
      QuotationNumber = obj.QuotationNumber,
      VATAmount = obj.VATAmount,
      VendorCode = obj.VendorCode,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ComputerAssetsMaintainceAccessory(ComputerAssetsMaintainceAccessoryDTO obj)
  {
    return new ComputerAssetsMaintainceAccessory
    {
      AccessoryGroupID = obj.AccessoryGroupID,
      Amount = obj.Amount,
      AssetDetailID = obj.AssetDetailID,
      DateOfAcquisition = obj.DateOfAcquisition,
      Description = obj.Description,
      InvoiceNumber = obj.InvoiceNumber,
      MaintainceID = obj.MaintainceID,
      Narration = obj.Narration,
      QuotationNumber = obj.QuotationNumber,
      VATAmount = obj.VATAmount,
      VendorCode = obj.VendorCode,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
  public override ComputerAssetsMaintainceAccessory? ToModel()
  {
    return (ComputerAssetsMaintainceAccessory)this;
  }
}
