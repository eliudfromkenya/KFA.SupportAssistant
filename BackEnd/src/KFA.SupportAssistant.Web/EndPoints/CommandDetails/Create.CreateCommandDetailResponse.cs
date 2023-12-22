namespace KFA.SupportAssistant.Web.EndPoints.CommandDetails;

public readonly struct CreateCommandDetailResponse(string? action, string? activeState, string? category, string? commandId, string? commandName, string? commandText, long? imageId, string? imagePath, bool? isEnabled, bool? isPublished, string? narration, string? shortcutKey, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? Action { get; } = action;
  public string? ActiveState { get; } = activeState;
  public string? Category { get; } = category;
  public string? CommandId { get; } = commandId;
  public string? CommandName { get; } = commandName;
  public string? CommandText { get; } = commandText;
  public long? ImageId { get; } = imageId;
  public string? ImagePath { get; } = imagePath;
  public bool? IsEnabled { get; } = isEnabled;
  public bool? IsPublished { get; } = isPublished;
  public string? Narration { get; } = narration;
  public string? ShortcutKey { get; } = shortcutKey;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
