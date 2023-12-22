using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class StockItemCodesRequestDTO : BaseDTO<StockItemCodesRequest>
{
  public string? AttandedBy { get; set; }
  public string? CostCentreCode { get; set; }
  public decimal CostPrice { get; set; }
  public string? Description { get; set; }
  public string? Distributor { get; set; }
  public string? ItemCode { get; set; }
  public string? Narration { get; set; }
  public string? RequestingUser { get; set; }
  public decimal SellingPrice { get; set; }
  public string? Status { get; set; }
  public string? Supplier { get; set; }
  public string? TimeAttended { get; set; }
  public DateTime? TimeOfRequest { get; set; }
  public string? UnitOfMeasure { get; set; }
  public override StockItemCodesRequest? ToModel()
  {
    return (StockItemCodesRequest)this;
  }

  public static implicit operator StockItemCodesRequestDTO(StockItemCodesRequest obj)
  {
    return new StockItemCodesRequestDTO
    {
      AttandedBy = obj.AttandedBy,
      CostCentreCode = obj.CostCentreCode,
      CostPrice = obj.CostPrice,
      Description = obj.Description,
      Distributor = obj.Distributor,
      ItemCode = obj.ItemCode,
      Narration = obj.Narration,
      RequestingUser = obj.RequestingUser,
      SellingPrice = obj.SellingPrice,
      Status = obj.Status,
      Supplier = obj.Supplier,
      TimeAttended = obj.TimeAttended,
      TimeOfRequest = obj.TimeOfRequest,
      UnitOfMeasure = obj.UnitOfMeasure,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator StockItemCodesRequest(StockItemCodesRequestDTO obj)
  {
    return new StockItemCodesRequest
    {
      AttandedBy = obj.AttandedBy,
      CostCentreCode = obj.CostCentreCode,
      CostPrice = obj.CostPrice,
      Description = obj.Description,
      Distributor = obj.Distributor,
      ItemCode = obj.ItemCode,
      Narration = obj.Narration,
      RequestingUser = obj.RequestingUser,
      SellingPrice = obj.SellingPrice,
      Status = obj.Status,
      Supplier = obj.Supplier,
      TimeAttended = obj.TimeAttended,
      TimeOfRequest = obj.TimeOfRequest,
      UnitOfMeasure = obj.UnitOfMeasure,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
