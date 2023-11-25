using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class PasswordSafeDTO : BaseDTO<PasswordSafe>
{
  public string? Details { get; set; }
  public string? Name { get; set; }
  public string? Password { get; set; }
  public string? Reminder { get; set; }
  public string? UsersVisibleTo { get; set; }
  public override PasswordSafe? ToModel()
  {
    return (PasswordSafe)this;
  }
  public static implicit operator PasswordSafeDTO(PasswordSafe obj)
  {
    return new PasswordSafeDTO
    {
      Details = obj.Details,
      Name = obj.Name,
      Password = obj.Password,
      Reminder = obj.Reminder,
      UsersVisibleTo = obj.UsersVisibleTo,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator PasswordSafe(PasswordSafeDTO obj)
  {
    return new PasswordSafe
    {
      Details = obj.Details,
      Name = obj.Name,
      Password = obj.Password,
      Reminder = obj.Reminder,
      UsersVisibleTo = obj.UsersVisibleTo,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
