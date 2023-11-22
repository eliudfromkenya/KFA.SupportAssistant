using Ardalis.SharedKernel;

namespace KFA.SupportAssistant.Globals.DataLayer;

public interface IBaseModel: IAggregateRoot
{
  string? Id { get; set; }
  bool? ___RecordIsSelected___ { get; set; }
  string? ___tableName___ { get; }
  long? ___DateUpdated___ { get; set; }
  long? ___DateInserted___ { get; set; }
  object? ___Tag___ { get; set; }
  byte? ___ModificationStatus___ { get; set; }
  IBaseDTO? ToDTO();
  string? AssignPrimaryKey();
}
