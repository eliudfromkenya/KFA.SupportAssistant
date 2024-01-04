using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_actual_budget_variances_batch_headers")]
public sealed record class ActualBudgetVariancesBatchHeader : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_actual_budget_variances_batch_headers";
  [MaxLength(255, ErrorMessage = "Please approved by must be 255 characters or less")]
  [Column("approved_by")]
  public string? ApprovedBy { get; init; }

  [Required]
  [Column("batch_key")]
  public override string? Id { get; init; }

  [MaxLength(255, ErrorMessage = "Please batch number must be 255 characters or less")]
  [Column("batch_number")]
  public string? BatchNumber { get; init; }

  [Column("cash_sales_amount")]
  public decimal CashSalesAmount { get; init; }

  [Column("computer_number_of_records")]
  public short ComputerNumberOfRecords { get; init; }

  [Column("computer_total_actual_amount")]
  public decimal ComputerTotalActualAmount { get; init; }

  [Column("computer_total_budget_amount")]
  public decimal ComputerTotalBudgetAmount { get; init; }

  [Required]
  [Column("cost_centre_code")]
  public string? CostCentreCode { get; init; }

  [ForeignKey(nameof(CostCentreCode))]
  public CostCentre? CostCentre { get; set; }
  [NotMapped]
  public string? CostCentre_Caption { get; set; }

  [Required]
  [MaxLength(10, ErrorMessage = "Please month must be 10 characters or less")]
  [Column("month")]
  public string? Month { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Column("number_of_records")]
  public short NumberOfRecords { get; init; }

  [MaxLength(255, ErrorMessage = "Please prepared by must be 255 characters or less")]
  [Column("prepared_by")]
  public string? PreparedBy { get; init; }

  [Column("purchaseses_amount")]
  public decimal PurchasesesAmount { get; init; }

  [Column("total_actual_amount")]
  public decimal TotalActualAmount { get; init; }

  [Column("total_budget_amount")]
  public decimal TotalBudgetAmount { get; init; }

  public override object ToBaseDTO()
  {
    return (ActualBudgetVariancesBatchHeaderDTO)this;
  }
}
