using KFA.SupportAssistant.Infrastructure.Models;
using KFA.SupportAssistant;

namespace KFA.SupportAssistant.UseCases.DTOs;
public record class UserRightDTO : BaseDTO<UserRight>
{
  public string? Description { get; set; }
  public string? Narration { get; set; }
  public string? ObjectName { get; set; }
  public string? RightId { get; set; }
  public string? RoleId { get; set; }
  public string? UserId { get; set; }
  public string? CommandId { get; set; }

  public override UserRight? ToModel()
  {
    return (UserRight)this;
  }

  public static implicit operator UserRightDTO(UserRight obj)
  {
    return new UserRightDTO
    {
      Description = obj.Description,
      Narration = obj.Narration,
      ObjectName = obj.ObjectName,
      RightId = obj.RightId,
      CommandId = obj.CommandId,
      RoleId = obj.RoleId,
      UserId = obj.UserId,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator UserRight(UserRightDTO obj)
  {
    return new UserRight
    {
      Description = obj.Description,
      Narration = obj.Narration,
      ObjectName = obj.ObjectName,
      CommandId = obj.CommandId,
      RightId = obj.RightId,
      RoleId = obj.RoleId,
      UserId = obj.UserId,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
