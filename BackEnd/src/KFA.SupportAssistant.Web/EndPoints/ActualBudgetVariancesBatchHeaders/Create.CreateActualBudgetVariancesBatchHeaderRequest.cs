using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariancesBatchHeaders;

public class CreateActualBudgetVariancesBatchHeaderRequest
{
  public const string Route = "/actual_budget_variances_batch_headers";
  public string? ApprovedBy { get; set; }

  [Required]
  public string? BatchKey { get; set; }

  public string? BatchNumber { get; set; }
  public decimal? CashSalesAmount { get; set; }
  public short? ComputerNumberOfRecords { get; set; }
  public decimal? ComputerTotalActualAmount { get; set; }
  public decimal? ComputerTotalBudgetAmount { get; set; }

  [Required]
  public string? CostCentreCode { get; set; }

  [Required]
  public string? Month { get; set; }

  public string? Narration { get; set; }
  public short? NumberOfRecords { get; set; }
  public string? PreparedBy { get; set; }
  public decimal? PurchasesesAmount { get; set; }
  public decimal? TotalActualAmount { get; set; }
  public decimal? TotalBudgetAmount { get; set; }
}
