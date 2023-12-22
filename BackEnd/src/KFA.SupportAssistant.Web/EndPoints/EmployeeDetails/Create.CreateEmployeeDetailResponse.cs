namespace KFA.SupportAssistant.Web.EndPoints.EmployeeDetails;

public readonly struct CreateEmployeeDetailResponse(decimal? amountDue, string? classification, string? costCentreCode, global::System.DateTime? date, string? email, string? employeeID, string? fullName, string? gender, string? groupNumber, string? idNumber, string? narration, string? payrollGroupID, string? payrollNumber, string? phoneNumber, global::System.DateTime? rejoinDate, string? remarks, decimal? retireeAmount, decimal? retrenchmentAmount, global::System.DateTime? retrenchmentDate, string? status, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public decimal? AmountDue { get; } = amountDue;
  public string? Classification { get; } = classification;
  public string? CostCentreCode { get; } = costCentreCode;
  public global::System.DateTime? Date { get; } = date;
  public string? Email { get; } = email;
  public string? EmployeeID { get; } = employeeID;
  public string? FullName { get; } = fullName;
  public string? Gender { get; } = gender;
  public string? GroupNumber { get; } = groupNumber;
  public string? IdNumber { get; } = idNumber;
  public string? Narration { get; } = narration;
  public string? PayrollGroupID { get; } = payrollGroupID;
  public string? PayrollNumber { get; } = payrollNumber;
  public string? PhoneNumber { get; } = phoneNumber;
  public global::System.DateTime? RejoinDate { get; } = rejoinDate;
  public string? Remarks { get; } = remarks;
  public decimal? RetireeAmount { get; } = retireeAmount;
  public decimal? RetrenchmentAmount { get; } = retrenchmentAmount;
  public global::System.DateTime? RetrenchmentDate { get; } = retrenchmentDate;
  public string? Status { get; } = status;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
