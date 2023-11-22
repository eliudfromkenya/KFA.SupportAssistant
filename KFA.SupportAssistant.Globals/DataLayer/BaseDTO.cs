using KFA.SupportAssistant.Globals.DataLayer;

public abstract record BaseDTO<T> : IBaseDTO where T : IBaseModel
{
  public string? Id { get; set; }
  public DateTime? DateUpdated___ { get; set; }
  public DateTime? DateInserted___ { get; set; }
  public object? ___Tag___ { get; set; }

  public abstract IBaseModel? ToModel();
}
