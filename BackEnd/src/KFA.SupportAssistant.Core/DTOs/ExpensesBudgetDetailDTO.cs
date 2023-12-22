using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ExpensesBudgetDetailDTO : BaseDTO<ExpensesBudgetDetail>
{
  public string? BasisOfCalculation { get; set; }
  public string? BatchKey { get; set; }
  public string? LedgerAccountCode { get; set; }
  public decimal Month01 { get; set; }
  public decimal Month02 { get; set; }
  public decimal Month03 { get; set; }
  public decimal Month04 { get; set; }
  public decimal Month05 { get; set; }
  public decimal Month06 { get; set; }
  public decimal Month07 { get; set; }
  public decimal Month08 { get; set; }
  public decimal Month09 { get; set; }
  public decimal Month10 { get; set; }
  public decimal Month11 { get; set; }
  public decimal Month12 { get; set; }
  public string? Narration { get; set; }
  public override ExpensesBudgetDetail? ToModel()
  {
    return (ExpensesBudgetDetail)this;
  }

  public static implicit operator ExpensesBudgetDetailDTO(ExpensesBudgetDetail obj)
  {
    return new ExpensesBudgetDetailDTO
    {
      BasisOfCalculation = obj.BasisOfCalculation,
      BatchKey = obj.BatchKey,
      LedgerAccountCode = obj.LedgerAccountCode,
      Month01 = obj.Month01,
      Month02 = obj.Month02,
      Month03 = obj.Month03,
      Month04 = obj.Month04,
      Month05 = obj.Month05,
      Month06 = obj.Month06,
      Month07 = obj.Month07,
      Month08 = obj.Month08,
      Month09 = obj.Month09,
      Month10 = obj.Month10,
      Month11 = obj.Month11,
      Month12 = obj.Month12,
      Narration = obj.Narration,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ExpensesBudgetDetail(ExpensesBudgetDetailDTO obj)
  {
    return new ExpensesBudgetDetail
    {
      BasisOfCalculation = obj.BasisOfCalculation,
      BatchKey = obj.BatchKey,
      LedgerAccountCode = obj.LedgerAccountCode,
      Month01 = obj.Month01,
      Month02 = obj.Month02,
      Month03 = obj.Month03,
      Month04 = obj.Month04,
      Month05 = obj.Month05,
      Month06 = obj.Month06,
      Month07 = obj.Month07,
      Month08 = obj.Month08,
      Month09 = obj.Month09,
      Month10 = obj.Month10,
      Month11 = obj.Month11,
      Month12 = obj.Month12,
      Narration = obj.Narration,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
