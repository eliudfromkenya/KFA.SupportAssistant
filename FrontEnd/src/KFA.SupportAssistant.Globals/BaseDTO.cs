using KFA.SupportAssistant.Globals;

public abstract record BaseDTO
{
  public string? Id { get; set; }
  public DateTime? DateUpdated___ { get; set; }
  public DateTime? DateInserted___ { get; set; }
  public object? ___Tag___ { get; set; }
}
