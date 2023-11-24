using KFA.SupportAssistant.Infrastructure.Models;
using KFA.SupportAssistant;

namespace KFA.SupportAssistant.UseCases.DTOs;
public record class QRRequestItemDTO : BaseDTO<QRRequestItem>
{
  public string? CashSaleNumber { get; set; }
  public string? CostCentreCode { get; set; }
  public string? HsCode { get; set; }
  public string? HsName { get; set; }
  public string? ItemCode { get; set; }
  public string? ItemName { get; set; }
  public string? Narration { get; set; }
  public decimal PercentageDiscount { get; set; }
  public decimal Quantity { get; set; }
  public string? RequestID { get; set; }
  public DateTime Time { get; set; }
  public decimal TotalAmount { get; set; }
  public string? UnitOfMeasure { get; set; }
  public decimal UnitPrice { get; set; }
  public decimal VATAmount { get; set; }
  public string? VATClass { get; set; }
  public override QRRequestItem? ToModel()
  {
    return (QRRequestItem)this;
  }
  public static implicit operator QRRequestItemDTO(QRRequestItem obj)
  {
    return new QRRequestItemDTO
    {
      CashSaleNumber = obj.CashSaleNumber,
      CostCentreCode = obj.CostCentreCode,
      HsCode = obj.HsCode,
      HsName = obj.HsName,
      ItemCode = obj.ItemCode,
      ItemName = obj.ItemName,
      Narration = obj.Narration,
      PercentageDiscount = obj.PercentageDiscount,
      Quantity = obj.Quantity,
      RequestID = obj.RequestID,
      Time = obj.Time,
      TotalAmount = obj.TotalAmount,
      UnitOfMeasure = obj.UnitOfMeasure,
      UnitPrice = obj.UnitPrice,
      VATAmount = obj.VATAmount,
      VATClass = obj.VATClass,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator QRRequestItem(QRRequestItemDTO obj)
  {
    return new QRRequestItem
    {
      CashSaleNumber = obj.CashSaleNumber,
      CostCentreCode = obj.CostCentreCode,
      HsCode = obj.HsCode,
      HsName = obj.HsName,
      ItemCode = obj.ItemCode,
      ItemName = obj.ItemName,
      Narration = obj.Narration,
      PercentageDiscount = obj.PercentageDiscount,
      Quantity = obj.Quantity,
      RequestID = obj.RequestID,
      Time = obj.Time,
      TotalAmount = obj.TotalAmount,
      UnitOfMeasure = obj.UnitOfMeasure,
      UnitPrice = obj.UnitPrice,
      VATAmount = obj.VATAmount,
      VATClass = obj.VATClass,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
