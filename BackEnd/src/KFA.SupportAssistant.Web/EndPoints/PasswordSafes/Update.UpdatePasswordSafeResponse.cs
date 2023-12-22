namespace KFA.SupportAssistant.Web.EndPoints.PasswordSafes;

public class UpdatePasswordSafeResponse
{
  public UpdatePasswordSafeResponse(PasswordSafeRecord passwordSafe)
  {
    PasswordSafe = passwordSafe;
  }

  public PasswordSafeRecord PasswordSafe { get; set; }
}
