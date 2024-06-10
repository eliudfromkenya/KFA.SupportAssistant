namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAssignments;

public readonly struct CreateComputerAssetAssignmentResponse(string? assetID, string? assignedUser, global::System.DateTime? assignmentDate, string? assignmentType, string? assignmentID, string? costCentreCode, string? narration, string? payrollNumber, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AssetID { get; } = assetID;
  public string? AssignedUser { get; } = assignedUser;
  public global::System.DateTime? AssignmentDate { get; } = assignmentDate;
  public string? AssignmentType { get; } = assignmentType;
  public string? AssignmentID { get; } = assignmentID;
  public string? CostCentreCode { get; } = costCentreCode;
  public string? Narration { get; } = narration;
  public string? PayrollNumber { get; } = payrollNumber;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
