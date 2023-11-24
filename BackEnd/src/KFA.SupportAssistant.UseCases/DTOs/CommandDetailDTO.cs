using KFA.SupportAssistant;
using KFA.SupportAssistant.Infrastructure.Models;

namespace KFA.SupportAssistant.UseCases.DTOs;
public record class CommandDetailDTO : BaseDTO<CommandDetail>
{
  public string? Action { get; set; }
  public string? ActiveState { get; set; }
  public string? Category { get; set; }
  public string? CommandName { get; set; }
  public string? CommandText { get; set; }
  public long ImageId { get; set; }
  public string? ImagePath { get; set; }
  public bool IsEnabled { get; set; }
  public bool IsPublished { get; set; }
  public string? Narration { get; set; }
  public string? ShortcutKey { get; set; }
  public override CommandDetail? ToModel()
  {
    return (CommandDetail)this;
  }

  public static implicit operator CommandDetailDTO(CommandDetail obj)
  {
    return new CommandDetailDTO
    {
      Action = obj.Action,
      ActiveState = obj.ActiveState,
      Category = obj.Category,
      CommandName = obj.CommandName,
      CommandText = obj.CommandText,
      ImageId = obj.ImageId,
      ImagePath = obj.ImagePath,
      IsEnabled = obj.IsEnabled,
      IsPublished = obj.IsPublished,
      Narration = obj.Narration,
      ShortcutKey = obj.ShortcutKey,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator CommandDetail(CommandDetailDTO obj)
  {
    return new CommandDetail
    {
      Action = obj.Action,
      ActiveState = obj.ActiveState,
      Category = obj.Category,
      CommandName = obj.CommandName,
      CommandText = obj.CommandText,
      ImageId = obj.ImageId,
      ImagePath = obj.ImagePath,
      IsEnabled = obj.IsEnabled,
      IsPublished = obj.IsPublished,
      Narration = obj.Narration,
      ShortcutKey = obj.ShortcutKey,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
