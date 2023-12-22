namespace KFA.SupportAssistant.Web.EndPoints.ItemGroups;

public readonly struct CreateItemGroupResponse(string? groupId, string? name, string? parentGroupId, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? GroupId { get; } = groupId;
  public string? Name { get; } = name;
  public string? ParentGroupId { get; } = parentGroupId;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
