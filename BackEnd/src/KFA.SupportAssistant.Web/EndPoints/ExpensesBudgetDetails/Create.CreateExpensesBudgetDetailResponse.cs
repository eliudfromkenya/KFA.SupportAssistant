namespace KFA.SupportAssistant.Web.EndPoints.ExpensesBudgetDetails;

public readonly struct CreateExpensesBudgetDetailResponse(string? basisOfCalculation, string? batchKey, string? expenseBudgetDetailId, string? ledgerAccountCode, decimal? month01, decimal? month02, decimal? month03, decimal? month04, decimal? month05, decimal? month06, decimal? month07, decimal? month08, decimal? month09, decimal? month10, decimal? month11, decimal? month12, string? narration, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? BasisOfCalculation { get; } = basisOfCalculation;
  public string? BatchKey { get; } = batchKey;
  public string? ExpenseBudgetDetailId { get; } = expenseBudgetDetailId;
  public string? LedgerAccountCode { get; } = ledgerAccountCode;
  public decimal? Month01 { get; } = month01;
  public decimal? Month02 { get; } = month02;
  public decimal? Month03 { get; } = month03;
  public decimal? Month04 { get; } = month04;
  public decimal? Month05 { get; } = month05;
  public decimal? Month06 { get; } = month06;
  public decimal? Month07 { get; } = month07;
  public decimal? Month08 { get; } = month08;
  public decimal? Month09 { get; } = month09;
  public decimal? Month10 { get; } = month10;
  public decimal? Month11 { get; } = month11;
  public decimal? Month12 { get; } = month12;
  public string? Narration { get; } = narration;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
