using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class SystemUserDTO : BaseDTO<SystemUser>
{
  public string? Contact { get; set; }
  public string? EmailAddress { get; set; }
  public DateTime ExpirationDate { get; set; }
  public bool IsActive { get; set; }
  public DateTime MaturityDate { get; set; }
  public string? NameOfTheUser { get; set; }
  public string? Narration { get; set; }
  //public string? Password { get; set; }
  public string? RoleId { get; set; }
  //public string? UserNumber { get; set; }
  public string? Username { get; set; }
  public override SystemUser? ToModel()
  {
    return (SystemUser)this;
  }
  public static implicit operator SystemUserDTO(SystemUser obj)
  {
    return new SystemUserDTO
    {
      Contact = obj.Contact,
      EmailAddress = obj.EmailAddress,
      ExpirationDate = obj.ExpirationDate,
      IsActive = obj.IsActive,
      MaturityDate = obj.MaturityDate,
      NameOfTheUser = obj.NameOfTheUser,
      Narration = obj.Narration,
      // Password = obj.Password,
      RoleId = obj.RoleId,
      //UserNumber = obj.UserNumber,
      Username = obj.Username,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator SystemUser(SystemUserDTO obj)
  {
    return new SystemUser
    {
      Contact = obj.Contact,
      EmailAddress = obj.EmailAddress,
      ExpirationDate = obj.ExpirationDate,
      IsActive = obj.IsActive,
      MaturityDate = obj.MaturityDate,
      NameOfTheUser = obj.NameOfTheUser,
      Narration = obj.Narration,
      //Password = obj.Password,
      RoleId = obj.RoleId,
      // UserNumber = obj.UserNumber,
      Username = obj.Username,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
