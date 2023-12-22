namespace KFA.SupportAssistant.Web.EndPoints.UserRights;

public readonly struct CreateUserRightResponse(string? description, string? narration, string? objectName, string? rightId, string? roleId, string? userId, string? userRightId, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? Description { get; } = description;
  public string? Narration { get; } = narration;
  public string? ObjectName { get; } = objectName;
  public string? RightId { get; } = rightId;
  public string? RoleId { get; } = roleId;
  public string? UserId { get; } = userId;
  public string? UserRightId { get; } = userRightId;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
