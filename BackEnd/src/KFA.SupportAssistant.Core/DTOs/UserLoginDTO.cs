using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class UserLoginDTO : BaseDTO<UserLogin>
{
  public string? DeviceId { get; set; }
  public DateTime FromDate { get; set; }
  public string? Narration { get; set; }
  public DateTime UptoDate { get; set; }
  public string? UserId { get; set; }
  public override UserLogin? ToModel()
  {
    return (UserLogin)this;
  }
  public static implicit operator UserLoginDTO(UserLogin obj)
  {
    return new UserLoginDTO
    {
      DeviceId = obj.DeviceId,
      FromDate = obj.FromDate,
      Narration = obj.Narration,
      UptoDate = obj.UptoDate,
      UserId = obj.UserId,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator UserLogin(UserLoginDTO obj)
  {
    return new UserLogin
    {
      DeviceId = obj.DeviceId,
      FromDate = obj.FromDate,
      Narration = obj.Narration,
      UptoDate = obj.UptoDate,
      UserId = obj.UserId,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
