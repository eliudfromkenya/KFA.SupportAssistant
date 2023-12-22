using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class SalesBudgetBatchHeaderDTO : BaseDTO<SalesBudgetBatchHeader>
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
  public decimal TotalQuantity { get; set; }
  public override SalesBudgetBatchHeader? ToModel()
  {
    return (SalesBudgetBatchHeader)this;
  }

  public static implicit operator SalesBudgetBatchHeaderDTO(SalesBudgetBatchHeader obj)
  {
    return new SalesBudgetBatchHeaderDTO
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
      TotalQuantity = obj.TotalQuantity,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator SalesBudgetBatchHeader(SalesBudgetBatchHeaderDTO obj)
  {
    return new SalesBudgetBatchHeader
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
      TotalQuantity = obj.TotalQuantity,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
