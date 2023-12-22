namespace KFA.SupportAssistant.Web.EndPoints.UserRights;

public class UpdateUserRightResponse
{
  public UpdateUserRightResponse(UserRightRecord userRight)
  {
    UserRight = userRight;
  }

  public UserRightRecord UserRight { get; set; }
}
