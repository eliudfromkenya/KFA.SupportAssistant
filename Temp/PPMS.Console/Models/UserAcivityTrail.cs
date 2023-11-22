using System.ComponentModel.DataAnnotations.Schema;

namespace PPMS.Console.Models
{

  [Table("sys_user_audit_trail")]
  public class UserAcivityTrail
  {
    [Column("rec_id")]
    public string RecId { get; set; }

    [Column("table_name")]
    public string TableName { get; set; }

    [Column("record_id")]
    public string RecordId { get; set; }

    [Column("change_body")]
    public string ChangeBody { get; set; }

    [Column("activity_type_id")]
    public byte AtivityId { get; set; }

    [Column("user_login_id")]
    public string LoginId { get; set; }

    [Column("narration")]
    public string Narration { get; set; }
  }
}