using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ActualBudgetVarianceDTO : BaseDTO<ActualBudgetVariance>
{
  public decimal ActualValue { get; set; }
  public string? BatchKey { get; set; }
  public decimal BudgetValue { get; set; }
  public string? Comment { get; set; }
  public string? Description { get; set; }
  public string? Field1 { get; set; }
  public string? Field2 { get; set; }
  public string? Field3 { get; set; }
  public string? LedgerCode { get; set; }
  public string? LedgerCostCentreCode { get; set; }
  public string? Narration { get; set; }
  public override ActualBudgetVariance? ToModel()
  {
    return (ActualBudgetVariance)this;
  }

  public static implicit operator ActualBudgetVarianceDTO(ActualBudgetVariance obj)
  {
    return new ActualBudgetVarianceDTO
    {
      ActualValue = obj.ActualValue,
      BatchKey = obj.BatchKey,
      BudgetValue = obj.BudgetValue,
      Comment = obj.Comment,
      Description = obj.Description,
      Field1 = obj.Field1,
      Field2 = obj.Field2,
      Field3 = obj.Field3,
      LedgerCode = obj.LedgerCode,
      LedgerCostCentreCode = obj.LedgerCostCentreCode,
      Narration = obj.Narration,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ActualBudgetVariance(ActualBudgetVarianceDTO obj)
  {
    return new ActualBudgetVariance
    {
      ActualValue = obj.ActualValue,
      BatchKey = obj.BatchKey,
      BudgetValue = obj.BudgetValue,
      Comment = obj.Comment,
      Description = obj.Description,
      Field1 = obj.Field1,
      Field2 = obj.Field2,
      Field3 = obj.Field3,
      LedgerCode = obj.LedgerCode,
      LedgerCostCentreCode = obj.LedgerCostCentreCode,
      Narration = obj.Narration,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
