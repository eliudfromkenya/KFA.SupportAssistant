using KFA.SupportAssistant.Core.DataLayer.Types;
using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class UserAuditTrailDTO : BaseDTO<UserAuditTrail>
{
  public DateTime ActivityDate { get; set; }
  public UserActivities ActivityEnumNumber { get; set; }
  public string? Category { get; set; }
  public string? CommandId { get; set; }
  public string? Data { get; set; }
  public string? Description { get; set; }
  public string? LoginId { get; set; }
  public string? Narration { get; set; }
  public string? OldValues { get; set; }
  public override UserAuditTrail? ToModel()
  {
    return (UserAuditTrail)this;
  }
  public static implicit operator UserAuditTrailDTO(UserAuditTrail obj)
  {
    return new UserAuditTrailDTO
    {
      ActivityDate = obj.ActivityDate,
      ActivityEnumNumber = obj.ActivityEnumNumber,
      Category = obj.Category,
      CommandId = obj.CommandId,
      Data = obj.Data,
      Description = obj.Description,
      LoginId = obj.LoginId,
      Narration = obj.Narration,
      OldValues = obj.OldValues,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator UserAuditTrail(UserAuditTrailDTO obj)
  {
    return new UserAuditTrail
    {
      ActivityDate = obj.ActivityDate,
      ActivityEnumNumber = obj.ActivityEnumNumber,
      Category = obj.Category,
      CommandId = obj.CommandId,
      Data = obj.Data,
      Description = obj.Description,
      LoginId = obj.LoginId,
      Narration = obj.Narration,
      OldValues = obj.OldValues,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
