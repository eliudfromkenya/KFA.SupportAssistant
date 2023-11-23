using KFA.DynamicsAssistant.Infrastructure.Models;
using KFA.SupportAssistant;

namespace KFA.SupportAssistant.UseCases.DTOs;
public record class VerificationDTO : BaseDTO<Verification>
{
  public DateTime DateOfVerification { get; set; }
  public string? LoginId { get; set; }
  public string? Narration { get; set; }
  public long RecordId { get; set; }
  public string? TableName { get; set; }
  public string? VerificationName { get; set; }
  public long VerificationRecordId { get; set; }
  public long VerificationTypeId { get; set; }
  public override Verification? ToModel()
  {
    return (Verification)this;
  }
  public static implicit operator VerificationDTO(Verification obj)
  {
    return new VerificationDTO
    {
      DateOfVerification = obj.DateOfVerification,
      LoginId = obj.LoginId,
      Narration = obj.Narration,
      RecordId = obj.RecordId,
      TableName = obj.TableName,
      VerificationName = obj.VerificationName,
      VerificationRecordId = obj.VerificationRecordId,
      VerificationTypeId = obj.VerificationTypeId,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator Verification(VerificationDTO obj)
  {
    return new Verification
    {
      DateOfVerification = obj.DateOfVerification,
      LoginId = obj.LoginId,
      Narration = obj.Narration,
      RecordId = obj.RecordId,
      TableName = obj.TableName,
      VerificationName = obj.VerificationName,
      VerificationRecordId = obj.VerificationRecordId,
      VerificationTypeId = obj.VerificationTypeId,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
