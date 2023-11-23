using KFA.DynamicsAssistant.Infrastructure.Models;
using KFA.SupportAssistant;

namespace KFA.SupportAssistant.UseCases.DTOs;
public record class StockItemDTO : BaseDTO<StockItem>
{
  public string? Barcode { get; set; }
  public string? GroupId { get; set; }
  public string? ItemName { get; set; }
  public string? Narration { get; set; }
  public override StockItem? ToModel()
  {
    return (StockItem)this;
  }
  public static implicit operator StockItemDTO(StockItem obj)
  {
    return new StockItemDTO
    {
      Barcode = obj.Barcode,
      GroupId = obj.GroupId,
      ItemName = obj.ItemName,
      Narration = obj.Narration,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator StockItem(StockItemDTO obj)
  {
    return new StockItem
    {
      Barcode = obj.Barcode,
      GroupId = obj.GroupId,
      ItemName = obj.ItemName,
      Narration = obj.Narration,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
