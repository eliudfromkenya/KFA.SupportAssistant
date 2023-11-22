using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Globals.DataLayer;

public enum ModificationTypes
{
  None = 0,
  Updated = 1,
  Inserted = 2,
  Delete = 3
}

public abstract record class BaseModel : IBaseModel
{
  private string? _id;

  [Key]
  [MaxLength(___PrimaryMaxLength___)]
  public virtual string? Id { get => _id; set => _id = value; }

  public BaseModel(string? id = null) => _id = id;

  string? IBaseModel.AssignPrimaryKey()
  {
    //return Id = new IdGenerator().GetNextId(GetType());
    return "543";
  }

  public abstract IBaseDTO? ToDTO();

#pragma warning disable IDE1006 // Naming Styles
  internal const int ___PrimaryMaxLength___ = 20;
#pragma warning restore IDE1006 // Naming Styles

  [NotMapped]
  [JsonIgnore]
  public abstract string? ___tableName___ { get; protected set; }

  [NotMapped]
  [JsonIgnore]
  public bool? ___RecordIsSelected___ { get; set; }

  [Column("modification_status", Order = 103)]
  public byte? ___ModificationStatus___ { get; set; } = 1;

  [NotMapped]
  [JsonIgnore]
  public object? ___Tag___ { get; set; }

  [Column("date_added", Order = 100)]
  public long? ___DateInserted___ { get; set; }

  [Column("date_updated", Order = 101)]
  public long? ___DateUpdated___ { get; set; }
}
