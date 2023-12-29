namespace KFA.SupportAssistant.RCL.Models.Data;
public record class SystemUserDTO
{
  public string? Contact { get; set; }
  public string? EmailAddress { get; set; }
  public DateTime? ExpirationDate { get; set; }
  public bool? IsActive { get; set; }
  public DateTime? MaturityDate { get; set; }
  public string? NameOfTheUser { get; set; }
  public string? Narration { get; set; }
  //public string? Password { get; set; }
  //public string? ConfirmPassword { get; set; }
  public string? RoleId { get; set; }
  //public string? UserNumber { get; set; }
  public string? Username { get; set; }
  public string? RefreshToken { get; }
  public string? AccessToken { get; }
}

public record class SignupSystemUserDTO
{
  public string? Contact { get; set; }
  public string? EmailAddress { get; set; }
  public DateTime? ExpirationDate { get; set; }
  public bool? IsActive { get; set; }
  public string? NameOfTheUser { get; set; }
  public string? Narration { get; set; }
  public string? Password { get; set; }
  public string? ConfirmPassword { get; set; }
  public string? RoleId { get; set; }
  public string? Username { get; set; }
}


