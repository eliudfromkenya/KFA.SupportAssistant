namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetGroups;

public readonly struct CreateComputerAssetGroupResponse(bool? canBeAssigned, string? description, string? groupID, string? groupName, string? lastAssignedValue, string? parentGroupID, string? prefix, string? suffix, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public bool? CanBeAssigned { get; } = canBeAssigned;
  public string? Description { get; } = description;
  public string? GroupID { get; } = groupID;
  public string? GroupName { get; } = groupName;
  public string? LastAssignedValue { get; } = lastAssignedValue;
  public string? ParentGroupID { get; } = parentGroupID;
  public string? Prefix { get; } = prefix;
  public string? Suffix { get; } = suffix;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
