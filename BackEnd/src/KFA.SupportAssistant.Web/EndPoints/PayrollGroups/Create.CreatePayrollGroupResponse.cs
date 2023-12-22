namespace KFA.SupportAssistant.Web.EndPoints.PayrollGroups;

public readonly struct CreatePayrollGroupResponse(string? groupID, string? groupName, string? narration, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? GroupID { get; } = groupID;
  public string? GroupName { get; } = groupName;
  public string? Narration { get; } = narration;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
