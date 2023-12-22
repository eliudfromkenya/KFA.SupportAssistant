using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ActualBudgetVariancesBatchHeaderDTO : BaseDTO<ActualBudgetVariancesBatchHeader>
{
  public string? ApprovedBy { get; set; }
  public string? BatchNumber { get; set; }
  public decimal CashSalesAmount { get; set; }
  public short ComputerNumberOfRecords { get; set; }
  public decimal ComputerTotalActualAmount { get; set; }
  public decimal ComputerTotalBudgetAmount { get; set; }
  public string? CostCentreCode { get; set; }
  public string? Month { get; set; }
  public string? Narration { get; set; }
  public short NumberOfRecords { get; set; }
  public string? PreparedBy { get; set; }
  public decimal PurchasesesAmount { get; set; }
  public decimal TotalActualAmount { get; set; }
  public decimal TotalBudgetAmount { get; set; }
  public override ActualBudgetVariancesBatchHeader ToModel()
  {
    return (ActualBudgetVariancesBatchHeader)this;
  }

  public static implicit operator ActualBudgetVariancesBatchHeaderDTO(ActualBudgetVariancesBatchHeader obj)
  {
    return new ActualBudgetVariancesBatchHeaderDTO
    {
      ApprovedBy = obj.ApprovedBy,
      BatchNumber = obj.BatchNumber,
      CashSalesAmount = obj.CashSalesAmount,
      ComputerNumberOfRecords = obj.ComputerNumberOfRecords,
      ComputerTotalActualAmount = obj.ComputerTotalActualAmount,
      ComputerTotalBudgetAmount = obj.ComputerTotalBudgetAmount,
      CostCentreCode = obj.CostCentreCode,
      Month = obj.Month,
      Narration = obj.Narration,
      NumberOfRecords = obj.NumberOfRecords,
      PreparedBy = obj.PreparedBy,
      PurchasesesAmount = obj.PurchasesesAmount,
      TotalActualAmount = obj.TotalActualAmount,
      TotalBudgetAmount = obj.TotalBudgetAmount,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ActualBudgetVariancesBatchHeader(ActualBudgetVariancesBatchHeaderDTO obj)
  {
    return new ActualBudgetVariancesBatchHeader
    {
      ApprovedBy = obj.ApprovedBy,
      BatchNumber = obj.BatchNumber,
      CashSalesAmount = obj.CashSalesAmount,
      ComputerNumberOfRecords = obj.ComputerNumberOfRecords,
      ComputerTotalActualAmount = obj.ComputerTotalActualAmount,
      ComputerTotalBudgetAmount = obj.ComputerTotalBudgetAmount,
      CostCentreCode = obj.CostCentreCode,
      Month = obj.Month,
      Narration = obj.Narration,
      NumberOfRecords = obj.NumberOfRecords,
      PreparedBy = obj.PreparedBy,
      PurchasesesAmount = obj.PurchasesesAmount,
      TotalActualAmount = obj.TotalActualAmount,
      TotalBudgetAmount = obj.TotalBudgetAmount,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
