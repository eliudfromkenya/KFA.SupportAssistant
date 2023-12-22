namespace KFA.SupportAssistant.Web.EndPoints.UserRoles;

public readonly struct CreateUserRoleResponse(global::System.DateTime? expirationDate, global::System.DateTime? maturityDate, string? narration, string? roleId, string? roleName, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public global::System.DateTime? ExpirationDate { get; } = expirationDate;
  public global::System.DateTime? MaturityDate { get; } = maturityDate;
  public string? Narration { get; } = narration;
  public string? RoleId { get; } = roleId;
  public string? RoleName { get; } = roleName;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
