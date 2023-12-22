using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class PriceChangeRequestDTO : BaseDTO<PriceChangeRequest>
{
  public string? AttandedBy { get; set; }
  public string? BatchNumber { get; set; }
  public string? CostCentreCode { get; set; }
  public string? CostPrice { get; set; }
  public string? ItemCode { get; set; }
  public string? Narration { get; set; }
  public string? RequestingUser { get; set; }
  public string? SellingPrice { get; set; }
  public string? Status { get; set; }
  public DateTime? TimeAttended { get; set; }
  public DateTime? TimeOfRequest { get; set; }
  public override PriceChangeRequest? ToModel()
  {
    return (PriceChangeRequest)this;
  }

  public static implicit operator PriceChangeRequestDTO(PriceChangeRequest obj)
  {
    return new PriceChangeRequestDTO
    {
      AttandedBy = obj.AttandedBy,
      BatchNumber = obj.BatchNumber,
      CostCentreCode = obj.CostCentreCode,
      CostPrice = obj.CostPrice,
      ItemCode = obj.ItemCode,
      Narration = obj.Narration,
      RequestingUser = obj.RequestingUser,
      SellingPrice = obj.SellingPrice,
      Status = obj.Status,
      TimeAttended = obj.TimeAttended,
      TimeOfRequest = obj.TimeOfRequest,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator PriceChangeRequest(PriceChangeRequestDTO obj)
  {
    return new PriceChangeRequest
    {
      AttandedBy = obj.AttandedBy,
      BatchNumber = obj.BatchNumber,
      CostCentreCode = obj.CostCentreCode,
      CostPrice = obj.CostPrice,
      ItemCode = obj.ItemCode,
      Narration = obj.Narration,
      RequestingUser = obj.RequestingUser,
      SellingPrice = obj.SellingPrice,
      Status = obj.Status,
      TimeAttended = obj.TimeAttended,
      TimeOfRequest = obj.TimeOfRequest,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
