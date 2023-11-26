using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.UserEndPoints;

public class RegisterRequest
{
  public const string Route = "/users/register";
  public string? Device { get; set; }
  public string? Contact { get; set; }
  public string? EmailAddress { get; set; }
  public DateTime ExpirationDate { get; set; }
  public bool IsActive { get; set; }

  [Required]
  public DateTime MaturityDate { get; set; }

  [Required]
  public string? NameOfTheUser { get; set; }

  public string? Narration { get; set; }

  [Required]
  public string? Password { get; set; }

  [Required]
  public string? Username { get; set; }
}
