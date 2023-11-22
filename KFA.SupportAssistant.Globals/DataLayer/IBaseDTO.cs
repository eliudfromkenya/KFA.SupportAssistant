namespace KFA.SupportAssistant.Globals.DataLayer;

public interface IBaseDTO
{
  string? Id { get; set; }
  DateTime? DateUpdated___ { get; set; }
  DateTime? DateInserted___ { get; set; }
  object? ___Tag___ { get; set; }
  IBaseModel? ToModel();
}
