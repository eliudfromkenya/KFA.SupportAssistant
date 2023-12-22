namespace KFA.SupportAssistant.Web.EndPoints.PasswordSafes;

public readonly struct CreatePasswordSafeResponse(string? details, string? name, string? password, string? passwordId, string? reminder, string? usersVisibleTo, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? Details { get; } = details;
  public string? Name { get; } = name;
  public string? Password { get; } = password;
  public string? PasswordId { get; } = passwordId;
  public string? Reminder { get; } = reminder;
  public string? UsersVisibleTo { get; } = usersVisibleTo;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
