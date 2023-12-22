namespace KFA.SupportAssistant.Web.EndPoints.SystemRights;

public class UpdateSystemRightResponse
{
  public UpdateSystemRightResponse(SystemRightRecord systemRight)
  {
    SystemRight = systemRight;
  }

  public SystemRightRecord SystemRight { get; set; }
}
