using KFA.DynamicsAssistant.Infrastructure.Models;
using KFA.SupportAssistant;

namespace KFA.SupportAssistant.UseCases.DTOs;
public record class UserRoleDTO : BaseDTO<UserRole>
{
  public DateTime ExpirationDate { get; set; }
  public DateTime MaturityDate { get; set; }
  public string? Narration { get; set; }
  public string? RoleName { get; set; }
  public short RoleNumber { get; set; }
  public override UserRole? ToModel()
  {
    return (UserRole)this;
  }
  public static implicit operator UserRoleDTO(UserRole obj)
  {
    return new UserRoleDTO
    {
      ExpirationDate = obj.ExpirationDate,
      MaturityDate = obj.MaturityDate,
      Narration = obj.Narration,
      RoleName = obj.RoleName,
      RoleNumber = obj.RoleNumber,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator UserRole(UserRoleDTO obj)
  {
    return new UserRole
    {
      ExpirationDate = obj.ExpirationDate,
      MaturityDate = obj.MaturityDate,
      Narration = obj.Narration,
      RoleName = obj.RoleName,
      RoleNumber = obj.RoleNumber,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
