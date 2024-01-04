using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_expense_budget_batch_headers")]
public sealed record class ExpenseBudgetBatchHeader : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_expense_budget_batch_headers";
  [MaxLength(255, ErrorMessage = "Please approved by must be 255 characters or less")]
  [Column("approved_by")]
  public string? ApprovedBy { get; init; }

  [Required]
  [Column("batch_key")]
  public override string? Id { get; init; }

  [MaxLength(255, ErrorMessage = "Please batch number must be 255 characters or less")]
  [Column("batch_number")]
  public string? BatchNumber { get; init; }

  [Column("computer_number_of_records")]
  public short ComputerNumberOfRecords { get; init; }

  [Column("computer_total_amount")]
  public decimal ComputerTotalAmount { get; init; }

  [Required]
  [Column("cost_centre_code")]
  public string? CostCentreCode { get; init; }

  [ForeignKey(nameof(CostCentreCode))]
  public CostCentre? CostCentre { get; set; }
  [NotMapped]
  public string? CostCentre_Caption { get; set; }

  [Column("date")]
  public global::System.DateTime Date { get; init; }

  [Required]
  [MaxLength(10, ErrorMessage = "Please month from must be 10 characters or less")]
  [Column("month_from")]
  public string? MonthFrom { get; init; }

  [Required]
  [MaxLength(10, ErrorMessage = "Please month to must be 10 characters or less")]
  [Column("month_to")]
  public string? MonthTo { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Column("number_of_records")]
  public short NumberOfRecords { get; init; }

  [MaxLength(255, ErrorMessage = "Please prepared by must be 255 characters or less")]
  [Column("prepared_by")]
  public string? PreparedBy { get; init; }

  [Column("total_amount")]
  public decimal TotalAmount { get; init; }

  public ICollection<ExpensesBudgetDetail>? ExpensesBudgetDetails { get; set; }

  public override object ToBaseDTO()
  {
    return (ExpenseBudgetBatchHeaderDTO)this;
  }
}
