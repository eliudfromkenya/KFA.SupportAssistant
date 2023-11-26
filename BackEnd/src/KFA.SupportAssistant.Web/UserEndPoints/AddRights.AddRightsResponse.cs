namespace KFA.SupportAssistant.Web.UserEndPoints;

public readonly struct AddRightsResponse
{
  public AddRightsResponse(string? id, string? userId, string? commandId, string? objectName, string? rightId) : this()
  {
    Id = id;
    this.UserId = userId;
    CommandId = commandId;
    ObjectName = objectName;
    RightId = rightId;
  }
  public string? UserId { get; }
  public string? Id { get; }
  public string? CommandId { get; }
  public string? ObjectName { get; }
  public string? RightId { get; }
}
