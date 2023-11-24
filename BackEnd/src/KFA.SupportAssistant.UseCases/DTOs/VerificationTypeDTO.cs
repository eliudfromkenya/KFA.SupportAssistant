using KFA.SupportAssistant.Infrastructure.Models;
using KFA.SupportAssistant;

namespace KFA.SupportAssistant.UseCases.DTOs;
public record class VerificationTypeDTO : BaseDTO<VerificationType>
{
  public string? Category { get; set; }
  public string? Narration { get; set; }
  public string? VerificationTypeName { get; set; }
  public override VerificationType? ToModel()
  {
    return (VerificationType)this;
  }
  public static implicit operator VerificationTypeDTO(VerificationType obj)
  {
    return new VerificationTypeDTO
    {
      Category = obj.Category,
      Narration = obj.Narration,
      VerificationTypeName = obj.VerificationTypeName,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator VerificationType(VerificationTypeDTO obj)
  {
    return new VerificationType
    {
      Category = obj.Category,
      Narration = obj.Narration,
      VerificationTypeName = obj.VerificationTypeName,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
