using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class VerificationRightDTO : BaseDTO<VerificationRight>
{
  public string? DeviceId { get; set; }
  public string? UserId { get; set; }
  public string? UserRoleId { get; set; }
  public long VerificationTypeId { get; set; }
  public override VerificationRight? ToModel()
  {
    return (VerificationRight)this;
  }

  public static implicit operator VerificationRightDTO(VerificationRight obj)
  {
    return new VerificationRightDTO
    {
      DeviceId = obj.DeviceId,
      UserId = obj.UserId,
      UserRoleId = obj.UserRoleId,
      VerificationTypeId = obj.VerificationTypeId,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator VerificationRight(VerificationRightDTO obj)
  {
    return new VerificationRight
    {
      DeviceId = obj.DeviceId,
      UserId = obj.UserId,
      UserRoleId = obj.UserRoleId,
      VerificationTypeId = obj.VerificationTypeId,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
