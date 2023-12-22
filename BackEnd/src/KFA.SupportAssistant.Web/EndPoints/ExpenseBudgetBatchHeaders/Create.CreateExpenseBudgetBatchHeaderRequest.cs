﻿using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ExpenseBudgetBatchHeaders;

public class CreateExpenseBudgetBatchHeaderRequest
{
  public const string Route = "/expense_budget_batch_headers";
  public string? ApprovedBy { get; set; }

  [Required]
  public string? BatchKey { get; set; }

  public string? BatchNumber { get; set; }
  public short? ComputerNumberOfRecords { get; set; }
  public decimal? ComputerTotalAmount { get; set; }

  [Required]
  public string? CostCentreCode { get; set; }

  public global::System.DateTime? Date { get; set; }

  [Required]
  public string? MonthFrom { get; set; }

  [Required]
  public string? MonthTo { get; set; }

  public string? Narration { get; set; }
  public short? NumberOfRecords { get; set; }
  public string? PreparedBy { get; set; }
  public decimal? TotalAmount { get; set; }
}