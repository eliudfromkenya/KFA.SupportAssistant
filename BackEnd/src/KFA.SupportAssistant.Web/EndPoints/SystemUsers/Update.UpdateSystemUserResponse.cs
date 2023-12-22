namespace KFA.SupportAssistant.Web.EndPoints.SystemUsers;

public class UpdateSystemUserResponse
{
  public UpdateSystemUserResponse(SystemUserRecord systemUser)
  {
    SystemUser = systemUser;
  }

  public SystemUserRecord SystemUser { get; set; }
}
