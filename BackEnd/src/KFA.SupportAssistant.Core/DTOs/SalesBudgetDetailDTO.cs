using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class SalesBudgetDetailDTO : BaseDTO<SalesBudgetDetail>
{
  public string? BatchKey { get; set; }
  public string? ItemCode { get; set; }
  public byte Month { get; set; }
  public decimal Month01Quantity { get; set; }
  public decimal Month02Quantity { get; set; }
  public decimal Month03Quantity { get; set; }
  public decimal Month04Quantity { get; set; }
  public decimal Month05Quantity { get; set; }
  public decimal Month06Quantity { get; set; }
  public decimal Month07Quantity { get; set; }
  public decimal Month08Quantity { get; set; }
  public decimal Month09Quantity { get; set; }
  public decimal Month10Quantity { get; set; }
  public decimal Month11Quantity { get; set; }
  public decimal Month12Quantity { get; set; }
  public string? Narration { get; set; }
  public decimal SellingPrice { get; set; }
  public decimal UnitCostPrice { get; set; }
  public override SalesBudgetDetail? ToModel()
  {
    return (SalesBudgetDetail)this;
  }

  public static implicit operator SalesBudgetDetailDTO(SalesBudgetDetail obj)
  {
    return new SalesBudgetDetailDTO
    {
      BatchKey = obj.BatchKey,
      ItemCode = obj.ItemCode,
      Month = obj.Month,
      Month01Quantity = obj.Month01Quantity,
      Month02Quantity = obj.Month02Quantity,
      Month03Quantity = obj.Month03Quantity,
      Month04Quantity = obj.Month04Quantity,
      Month05Quantity = obj.Month05Quantity,
      Month06Quantity = obj.Month06Quantity,
      Month07Quantity = obj.Month07Quantity,
      Month08Quantity = obj.Month08Quantity,
      Month09Quantity = obj.Month09Quantity,
      Month10Quantity = obj.Month10Quantity,
      Month11Quantity = obj.Month11Quantity,
      Month12Quantity = obj.Month12Quantity,
      Narration = obj.Narration,
      SellingPrice = obj.SellingPrice,
      UnitCostPrice = obj.UnitCostPrice,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator SalesBudgetDetail(SalesBudgetDetailDTO obj)
  {
    return new SalesBudgetDetail
    {
      BatchKey = obj.BatchKey,
      ItemCode = obj.ItemCode,
      Month = obj.Month,
      Month01Quantity = obj.Month01Quantity,
      Month02Quantity = obj.Month02Quantity,
      Month03Quantity = obj.Month03Quantity,
      Month04Quantity = obj.Month04Quantity,
      Month05Quantity = obj.Month05Quantity,
      Month06Quantity = obj.Month06Quantity,
      Month07Quantity = obj.Month07Quantity,
      Month08Quantity = obj.Month08Quantity,
      Month09Quantity = obj.Month09Quantity,
      Month10Quantity = obj.Month10Quantity,
      Month11Quantity = obj.Month11Quantity,
      Month12Quantity = obj.Month12Quantity,
      Narration = obj.Narration,
      SellingPrice = obj.SellingPrice,
      UnitCostPrice = obj.UnitCostPrice,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
