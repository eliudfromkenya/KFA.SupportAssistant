namespace KFA.SupportAssistant.Web.EndPoints.StaffGroups;

public readonly struct CreateStaffGroupResponse(string? description, string? groupNumber, bool? isActive, string? narration, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? Description { get; } = description;
  public string? GroupNumber { get; } = groupNumber;
  public bool? IsActive { get; } = isActive;
  public string? Narration { get; } = narration;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
