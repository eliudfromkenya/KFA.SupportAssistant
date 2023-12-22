using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_expenses_budget_details")]
public sealed record class ExpensesBudgetDetail : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_expenses_budget_details";
  [MaxLength(255, ErrorMessage = "Please basis of calculation must be 255 characters or less")]
  [Column("basis_of_calculation")]
  public string? BasisOfCalculation { get; init; }

  [Column("batch_key")]
  public string? BatchKey { get; init; }

  [ForeignKey(nameof(BatchKey))]
  public ExpenseBudgetBatchHeader? Batch { get; set; }
  [NotMapped]
  public string? Batch_Caption { get; set; }
  [Required]
  //    [Index(IsUnique = true)]
  [Column("expense_budget_detail_id")]
  public override string? Id { get; set; }

  [Column("ledger_account_code")]
  public string? LedgerAccountCode { get; init; }

  [Column("month_01")]
  public decimal Month01 { get; init; }

  [Column("month_02")]
  public decimal Month02 { get; init; }

  [Column("month_03")]
  public decimal Month03 { get; init; }

  [Column("month_04")]
  public decimal Month04 { get; init; }

  [Column("month_05")]
  public decimal Month05 { get; init; }

  [Column("month_06")]
  public decimal Month06 { get; init; }

  [Column("month_07")]
  public decimal Month07 { get; init; }

  [Column("month_08")]
  public decimal Month08 { get; init; }

  [Column("month_09")]
  public decimal Month09 { get; init; }

  [Column("month_10")]
  public decimal Month10 { get; init; }

  [Column("month_11")]
  public decimal Month11 { get; init; }

  [Column("month_12")]
  public decimal Month12 { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  public ICollection<LedgerAccount>? LedgerAccounts { get; set; }
  public override object ToBaseDTO()
  {
    return (ExpensesBudgetDetailDTO)this;
  }
}
