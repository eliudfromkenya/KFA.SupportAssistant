using KFA.SupportAssistant.Infrastructure.Models;
using KFA.SupportAssistant;

namespace KFA.SupportAssistant.UseCases.DTOs;
public record class SystemRightDTO : BaseDTO<SystemRight>
{
  public bool IsCompulsory { get; set; }
  public string? Narration { get; set; }
  public string? RightName { get; set; }
  public override SystemRight? ToModel()
  {
    return (SystemRight)this;
  }
  public static implicit operator SystemRightDTO(SystemRight obj)
  {
    return new SystemRightDTO
    {
      IsCompulsory = obj.IsCompulsory,
      Narration = obj.Narration,
      RightName = obj.RightName,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator SystemRight(SystemRightDTO obj)
  {
    return new SystemRight
    {
      IsCompulsory = obj.IsCompulsory,
      Narration = obj.Narration,
      RightName = obj.RightName,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
