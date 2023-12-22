using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class PurchasesBudgetDetailDTO : BaseDTO<PurchasesBudgetDetail>
{
  public long BatchKey { get; set; }
  public decimal BuyingPrice { get; set; }
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
  public decimal UnitCostPrice { get; set; }
  public override PurchasesBudgetDetail? ToModel()
  {
    return (PurchasesBudgetDetail)this;
  }

  public static implicit operator PurchasesBudgetDetailDTO(PurchasesBudgetDetail obj)
  {
    return new PurchasesBudgetDetailDTO
    {
      BatchKey = obj.BatchKey,
      BuyingPrice = obj.BuyingPrice,
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
      UnitCostPrice = obj.UnitCostPrice,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator PurchasesBudgetDetail(PurchasesBudgetDetailDTO obj)
  {
    return new PurchasesBudgetDetail
    {
      BatchKey = obj.BatchKey,
      BuyingPrice = obj.BuyingPrice,
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
      UnitCostPrice = obj.UnitCostPrice,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
