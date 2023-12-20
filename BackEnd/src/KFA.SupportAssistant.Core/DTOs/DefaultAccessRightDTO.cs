using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class DefaultAccessRightDTO : BaseDTO<DefaultAccessRight>
{
  public string? Name { get; set; }
  public string? Rights { get; set; }
  public string? Type { get; set; }
  public string? Narration { get; set; }
  public override DefaultAccessRight? ToModel()
  {
    return (DefaultAccessRight)this;
  }
  public static implicit operator DefaultAccessRightDTO(DefaultAccessRight obj)
  {
    return new DefaultAccessRightDTO
    {
      Name = obj.Name,
      Type = obj.Type,
      Rights = obj.Rights,
      Narration = obj.Narration,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator DefaultAccessRight(DefaultAccessRightDTO obj)
  {
    return new DefaultAccessRight
    {
      Name = obj.Name,
      Type = obj.Type,
      Rights = obj.Rights,
      Narration = obj.Narration,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }

}
