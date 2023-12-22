using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ExpenseBudgetBatchHeaderDTO : BaseDTO<ExpenseBudgetBatchHeader>
{
  public string? ApprovedBy { get; set; }
  public string? BatchNumber { get; set; }
  public short ComputerNumberOfRecords { get; set; }
  public decimal ComputerTotalAmount { get; set; }
  public string? CostCentreCode { get; set; }
  public global::System.DateTime Date { get; set; }
  public string? MonthFrom { get; set; }
  public string? MonthTo { get; set; }
  public string? Narration { get; set; }
  public short NumberOfRecords { get; set; }
  public string? PreparedBy { get; set; }
  public decimal TotalAmount { get; set; }
  public override ExpenseBudgetBatchHeader? ToModel()
  {
    return (ExpenseBudgetBatchHeader)this;
  }

  public static implicit operator ExpenseBudgetBatchHeaderDTO(ExpenseBudgetBatchHeader obj)
  {
    return new ExpenseBudgetBatchHeaderDTO
    {
      ApprovedBy = obj.ApprovedBy,
      BatchNumber = obj.BatchNumber,
      ComputerNumberOfRecords = obj.ComputerNumberOfRecords,
      ComputerTotalAmount = obj.ComputerTotalAmount,
      CostCentreCode = obj.CostCentreCode,
      Date = obj.Date,
      MonthFrom = obj.MonthFrom,
      MonthTo = obj.MonthTo,
      Narration = obj.Narration,
      NumberOfRecords = obj.NumberOfRecords,
      PreparedBy = obj.PreparedBy,
      TotalAmount = obj.TotalAmount,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ExpenseBudgetBatchHeader(ExpenseBudgetBatchHeaderDTO obj)
  {
    return new ExpenseBudgetBatchHeader
    {
      ApprovedBy = obj.ApprovedBy,
      BatchNumber = obj.BatchNumber,
      ComputerNumberOfRecords = obj.ComputerNumberOfRecords,
      ComputerTotalAmount = obj.ComputerTotalAmount,
      CostCentreCode = obj.CostCentreCode,
      Date = obj.Date,
      MonthFrom = obj.MonthFrom,
      MonthTo = obj.MonthTo,
      Narration = obj.Narration,
      NumberOfRecords = obj.NumberOfRecords,
      PreparedBy = obj.PreparedBy,
      TotalAmount = obj.TotalAmount,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
