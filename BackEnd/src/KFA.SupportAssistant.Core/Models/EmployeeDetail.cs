using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_employee_details")]
public sealed record class EmployeeDetail : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_employee_details";
  [Column("amount_due")]
  [Encrypted]
  public string? AmountDue { get; init; }

  [MaxLength(5, ErrorMessage = "Please cost centre code must be 6 characters or less")]
  [Encrypted]
  [Column("cost_centre_code")]
  public string? CostCentreCode { get; init; }

  [Column("date")]
  [Encrypted]
  public string? Date { get; init; }

  [MaxLength(255, ErrorMessage = "Please remarks must be 255 characters or less")]
  [Column("remarks")]
  [Encrypted]
  public string? Remarks { get; init; }
  [MaxLength(255, ErrorMessage = "Please classfication must be 255 characters or less")]
  [Column("classfication")]
  [Encrypted]
  public string? Classfication { get; init; }
  [MaxLength(255, ErrorMessage = "Please email must be 255 characters or less")]
  [Column("email")]
  [Encrypted]
  public string? Email { get; init; }
  // [Required]
  [Column("employee_id")]
  public override string? Id { get; set; } = string.Empty;

  // [Required]
  [Encrypted]
  [MaxLength(255, ErrorMessage = "Please full name must be 255 characters or less")]
  [Column("full_name")]
  public string? FullName { get; init; }

  [MaxLength(8, ErrorMessage = "Please gender must be 8 characters or less")]
  [Encrypted]
  [Column("gender")]
  public string? Gender { get; init; }

  [MaxLength(15, ErrorMessage = "Please id number must be 15 characters or less")]
  [Encrypted]
  [Column("id_number")]
  public string? IdNumber { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Encrypted]
  [Column("narration")]
  public string? Narration { get; init; }

  [MaxLength(8, ErrorMessage = "Please payroll number must be 8 characters or less")]
  [Encrypted]
  [Column("payroll_number")]
  public string? PayrollNumber { get; init; }

  [MaxLength(25, ErrorMessage = "Please phone number must be 25 characters or less")]
  [Encrypted]
  [Column("phone_number")]
  public string? PhoneNumber { get; init; }

  [MaxLength(10, ErrorMessage = "Please status must be 10 characters or less")]
  [Encrypted]
  [Column("status")]
  public string? Status { get; init; }
  [Encrypted]
  public CostCentre? CostCentre { get; set; }
  [NotMapped]
  public string? CostCentre_Caption { get; set; }
  [Encrypted]
  [Column("group_number")]
  public string? GroupNumber { get; init; }

  [ForeignKey(nameof(GroupNumber))]
  public StaffGroup? Group { get; set; }
  [NotMapped]
  public string? Group_Caption { get; set; }
  [Encrypted]
  [Column("rejoin_date")]
  public string? RejoinDate { get; init; }
  [Encrypted]
  [Column("retiree_amount")]
  public string? RetireeAmount { get; init; }
  [Encrypted]
  [Column("retrenchment_amount")]
  public string? RetrenchmentAmount { get; init; }
  [Encrypted]
  [Column("retrenchment_date")]
  public string? RetrenchmentDate { get; init; }
  public ICollection<DuesPaymentDetail>? DuesPaymentDetails { get; set; }
  public override object ToBaseDTO()
  {
    return (EmployeeDetailDTO)this;
  }
}
