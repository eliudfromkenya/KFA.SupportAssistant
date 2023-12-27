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
  public decimal AmountDue { get; init; }

  [MaxLength(255, ErrorMessage = "Please classification must be 255 characters or less")]
  [Column("classfication")]
  public string? Classification { get; init; }

  [Column("cost_centre_code")]
  public string? CostCentreCode { get; init; }

  [ForeignKey(nameof(CostCentreCode))]
  public CostCentre? CostCentre { get; set; }
  [NotMapped]
  public string? CostCentre_Caption { get; set; }

  [Column("date")]
  public global::System.DateTime? Date { get; init; }

  [MaxLength(255, ErrorMessage = "Please email must be 255 characters or less")]
  [Column("email")]
  public string? Email { get; init; }

  [Required]
  [Column("employee_id")]
  public override string? Id { get; set; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please full name must be 255 characters or less")]
  [Column("full_name")]
  public string? FullName { get; init; }

  [MaxLength(8, ErrorMessage = "Please gender must be 8 characters or less")]
  [Column("gender")]
  public string? Gender { get; init; }

  [Column("group_number")]
  public string? GroupNumber { get; init; }

  [ForeignKey(nameof(GroupNumber))]
  public StaffGroup? Group { get; set; }
  [NotMapped]
  public string? Group_Caption { get; set; }

  [MaxLength(15, ErrorMessage = "Please id number must be 15 characters or less")]
  [Column("id_number")]
  public string? IdNumber { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Column("payroll_group_id")]
  public string? PayrollGroupID { get; init; }

  [ForeignKey(nameof(PayrollGroupID))]
  public PayrollGroup? PayrollGroup { get; set; }
  [NotMapped]
  public string? PayrollGroup_Caption { get; set; }

  [MaxLength(8, ErrorMessage = "Please payroll number must be 8 characters or less")]
  [Column("payroll_number")]
  public string? PayrollNumber { get; init; }

  [MaxLength(25, ErrorMessage = "Please phone number must be 25 characters or less")]
  [Column("phone_number")]
  public string? PhoneNumber { get; init; }

  [Column("rejoin_date")]
  public global::System.DateTime? RejoinDate { get; init; }

  [MaxLength(255, ErrorMessage = "Please remarks must be 255 characters or less")]
  [Column("remarks")]
  public string? Remarks { get; init; }

  [Column("retiree_amount")]
  public decimal? RetireeAmount { get; init; }

  [Column("retrenchment_amount")]
  public decimal? RetrenchmentAmount { get; init; }

  [Column("retrenchment_date")]
  public global::System.DateTime? RetrenchmentDate { get; init; }

  [MaxLength(10, ErrorMessage = "Please status must be 10 characters or less")]
  [Column("status")]
  public string? Status { get; init; }

  public ICollection<DuesPaymentDetail>? DuesPaymentDetails { get; set; }

  public override object ToBaseDTO()
  {
    return (EmployeeDetailDTO)this;
  }
}
