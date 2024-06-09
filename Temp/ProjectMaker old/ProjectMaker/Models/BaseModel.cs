using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectMaker.Models;


public enum ModificationTypes {
  None = 0,
  Updated = 1,
  Inserted = 2,
  Delete = 3
}

public abstract class BaseModel {
  [Key]
  public abstract string? Id { get; set; }

  // [NotMapped]
  //public abstract string tableName { get; protected set; }

  [Column ("date_added")]
  public long? DateInserted { get; private set; }

  [Column ("date_updated")]
  public long? DateUpdated { get; private set; }

  [Column ("originator_id")]
  public int? OriginatorId { get; private set; }

  static BaseModel () {
    
      }

  public long ConvertDateToNumber (DateTime dateTime) {
    return GetTimestamp (dateTime);
  }

  static long GetTimestamp (DateTime dateTime) {
    return
    Convert.ToInt64 (
      string.Format ("{0:yyyyMMddhhmmss}{1}0000000", dateTime, dateTime.Millisecond)
      .Substring (0, 17));
  }
}
