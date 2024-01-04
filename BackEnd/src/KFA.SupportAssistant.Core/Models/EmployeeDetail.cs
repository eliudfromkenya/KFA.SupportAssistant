using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_employee_details")]
public sealed record class EmployeeDetail : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_employee_details";
  [Column("amount_due")]
  [Encrypted]
  public string? AmountDue { get; init; } = string.Empty;
  [Encrypted]
  [MaxLength(255, ErrorMessage = "Please classification must be 255 characters or less")]
  [Column("classfication")]
  public string? Classification { get; init; } = string.Empty;

  [Column("cost_centre_code")]
  public string? CostCentreCode { get; init; }

  [ForeignKey(nameof(CostCentreCode))]
  public CostCentre? CostCentre { get; set; }
  [NotMapped]
  public string? CostCentre_Caption { get; set; }
  [Encrypted]
  [Column("date")]
  public string? Date { get; init; } = string.Empty;

  [MaxLength(255, ErrorMessage = "Please email must be 255 characters or less")]
  [Column("email")]
  [Encrypted]
  public string? Email { get; init; } = string.Empty;

  //[Required]
  [Column("employee_id")]
  public override string? Id { get; init; }
  [Encrypted]
  // [Required]
  [MaxLength(255, ErrorMessage = "Please full name must be 255 characters or less")]
  [Column("full_name")]
  public string? FullName { get; init; } = string.Empty;
  [Encrypted]
  [MaxLength(8, ErrorMessage = "Please gender must be 8 characters or less")]
  [Column("gender")]
  public string? Gender { get; init; } = string.Empty;
  [Column("group_number")]
  [Encrypted]
  public string? GroupNumber { get; init; }

  [ForeignKey(nameof(GroupNumber))]
  [JsonIgnore]
  public StaffGroup? Group { get; set; }
  [NotMapped]
  public string? Group_Caption { get; set; }

  [MaxLength(15, ErrorMessage = "Please id number must be 15 characters or less")]
  [Column("id_number")]
  [Encrypted]
  public string? IdNumber { get; init; } = string.Empty;

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  [Encrypted]
  public string? Narration { get; init; } = string.Empty;
  [Encrypted]
  [Column("payroll_group_id")]
  public string? PayrollGroupID { get; init; }

  [ForeignKey(nameof(PayrollGroupID))]
  [JsonIgnore]
  public PayrollGroup? PayrollGroup { get; set; }
  [NotMapped]
  public string? PayrollGroup_Caption { get; set; }

  [MaxLength(8, ErrorMessage = "Please payroll number must be 8 characters or less")]
  [Column("payroll_number")]
  [Encrypted]
  public string? PayrollNumber { get; init; } = string.Empty;

  [MaxLength(25, ErrorMessage = "Please phone number must be 25 characters or less")]
  [Column("phone_number")]
  [Encrypted]
  public string? PhoneNumber { get; init; } = string.Empty;

  [Column("rejoin_date")]
  [Encrypted]
  public string? RejoinDate { get; init; } = string.Empty;

  [MaxLength(255, ErrorMessage = "Please remarks must be 255 characters or less")]
  [Column("remarks")]
  [Encrypted]
  public string? Remarks { get; init; } = string.Empty;

  [Column("retiree_amount")]
  [Encrypted]
  public string? RetireeAmount { get; init; } = string.Empty;

  [Column("retrenchment_amount")]
  [Encrypted]
  public string? RetrenchmentAmount { get; init; } = string.Empty;

  [Column("retrenchment_date")]
  [Encrypted]
  public string? RetrenchmentDate { get; init; } = string.Empty;

  [MaxLength(10, ErrorMessage = "Please status must be 10 characters or less")]
  [Column("status")]
  [Encrypted]
  public string? Status { get; init; } = string.Empty;
  [JsonIgnore]
  public ICollection<DuesPaymentDetail>? DuesPaymentDetails { get; set; }

  public override object ToBaseDTO()
  {
    return (EmployeeDetailDTO)this;
  }
}
