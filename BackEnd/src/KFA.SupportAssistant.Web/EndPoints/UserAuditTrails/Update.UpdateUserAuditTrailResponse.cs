namespace KFA.SupportAssistant.Web.EndPoints.UserAuditTrails;

public class UpdateUserAuditTrailResponse
{
  public UpdateUserAuditTrailResponse(UserAuditTrailRecord userAuditTrail)
  {
    UserAuditTrail = userAuditTrail;
  }

  public UserAuditTrailRecord UserAuditTrail { get; set; }
}
