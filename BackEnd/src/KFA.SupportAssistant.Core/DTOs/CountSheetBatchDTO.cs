using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class CountSheetBatchDTO : BaseDTO<CountSheetBatch>
{
  public string? BatchNumber { get; set; }
  public string? ClassOfCard { get; set; }
  public short ComputerNumberOfRecords { get; set; }
  public decimal ComputerTotalAmount { get; set; }
  public string? CostCentreCode { get; set; }
  public global::System.DateTime Date { get; set; }
  public string? Month { get; set; }
  public string? Narration { get; set; }
  public short NoOfRecords { get; set; }
  public decimal TotalAmount { get; set; }
  public override CountSheetBatch? ToModel()
  {
    return (CountSheetBatch)this;
  }

  public static implicit operator CountSheetBatchDTO(CountSheetBatch obj)
  {
    return new CountSheetBatchDTO
    {
      BatchNumber = obj.BatchNumber,
      ClassOfCard = obj.ClassOfCard,
      ComputerNumberOfRecords = obj.ComputerNumberOfRecords,
      ComputerTotalAmount = obj.ComputerTotalAmount,
      CostCentreCode = obj.CostCentreCode,
      Date = obj.Date,
      Month = obj.Month,
      Narration = obj.Narration,
      NoOfRecords = obj.NoOfRecords,
      TotalAmount = obj.TotalAmount,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator CountSheetBatch(CountSheetBatchDTO obj)
  {
    return new CountSheetBatch
    {
      BatchNumber = obj.BatchNumber,
      ClassOfCard = obj.ClassOfCard,
      ComputerNumberOfRecords = obj.ComputerNumberOfRecords,
      ComputerTotalAmount = obj.ComputerTotalAmount,
      CostCentreCode = obj.CostCentreCode,
      Date = obj.Date,
      Month = obj.Month,
      Narration = obj.Narration,
      NoOfRecords = obj.NoOfRecords,
      TotalAmount = obj.TotalAmount,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
