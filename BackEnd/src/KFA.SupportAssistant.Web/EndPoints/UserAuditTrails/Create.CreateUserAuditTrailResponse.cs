using KFA.SupportAssistant.Core.DataLayer.Types;

namespace KFA.SupportAssistant.Web.EndPoints.UserAuditTrails;

public readonly struct CreateUserAuditTrailResponse(global::System.DateTime? activityDate, UserActivities? activityEnumNumber, string? auditId, string? category, string? commandId, string? data, string? description, string? loginId, string? narration, string? oldValues, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public global::System.DateTime? ActivityDate { get; } = activityDate;
  public UserActivities? ActivityEnumNumber { get; } = activityEnumNumber;
  public string? AuditId { get; } = auditId;
  public string? Category { get; } = category;
  public string? CommandId { get; } = commandId;
  public string? Data { get; } = data;
  public string? Description { get; } = description;
  public string? LoginId { get; } = loginId;
  public string? Narration { get; } = narration;
  public string? OldValues { get; } = oldValues;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
