using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class StockCountSheetDTO : BaseDTO<StockCountSheet>
{
  public decimal Actual { get; set; }
  public decimal AverageAgeMonths { get; set; }
  public long BatchKey { get; set; }
  public long CountSheetDocumentId { get; set; }
  public string? DocumentNumber { get; set; }
  public string? ItemCode { get; set; }
  public string? Narration { get; set; }
  public decimal QuantityOnHand { get; set; }
  public decimal QuantitySoldLast12Months { get; set; }
  public decimal SellingPrice { get; set; }
  public decimal StocksOver { get; set; }
  public decimal StocksShort { get; set; }
  public decimal UnitCostPrice { get; set; }
  public override StockCountSheet? ToModel()
  {
    return (StockCountSheet)this;
  }

  public static implicit operator StockCountSheetDTO(StockCountSheet obj)
  {
    return new StockCountSheetDTO
    {
      Actual = obj.Actual,
      AverageAgeMonths = obj.AverageAgeMonths,
      BatchKey = obj.BatchKey,
      CountSheetDocumentId = obj.CountSheetDocumentId,
      DocumentNumber = obj.DocumentNumber,
      ItemCode = obj.ItemCode,
      Narration = obj.Narration,
      QuantityOnHand = obj.QuantityOnHand,
      QuantitySoldLast12Months = obj.QuantitySoldLast12Months,
      SellingPrice = obj.SellingPrice,
      StocksOver = obj.StocksOver,
      StocksShort = obj.StocksShort,
      UnitCostPrice = obj.UnitCostPrice,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator StockCountSheet(StockCountSheetDTO obj)
  {
    return new StockCountSheet
    {
      Actual = obj.Actual,
      AverageAgeMonths = obj.AverageAgeMonths,
      BatchKey = obj.BatchKey,
      CountSheetDocumentId = obj.CountSheetDocumentId,
      DocumentNumber = obj.DocumentNumber,
      ItemCode = obj.ItemCode,
      Narration = obj.Narration,
      QuantityOnHand = obj.QuantityOnHand,
      QuantitySoldLast12Months = obj.QuantitySoldLast12Months,
      SellingPrice = obj.SellingPrice,
      StocksOver = obj.StocksOver,
      StocksShort = obj.StocksShort,
      UnitCostPrice = obj.UnitCostPrice,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
