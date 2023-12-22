namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariances;

public readonly struct CreateActualBudgetVarianceResponse(string? actualBudgetID, decimal? actualValue, string? batchKey, decimal? budgetValue, string? comment, string? description, string? field1, string? field2, string? field3, string? ledgerCode, string? ledgerCostCentreCode, string? narration, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? ActualBudgetID { get; } = actualBudgetID;
  public decimal? ActualValue { get; } = actualValue;
  public string? BatchKey { get; } = batchKey;
  public decimal? BudgetValue { get; } = budgetValue;
  public string? Comment { get; } = comment;
  public string? Description { get; } = description;
  public string? Field1 { get; } = field1;
  public string? Field2 { get; } = field2;
  public string? Field3 { get; } = field3;
  public string? LedgerCode { get; } = ledgerCode;
  public string? LedgerCostCentreCode { get; } = ledgerCostCentreCode;
  public string? Narration { get; } = narration;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
