using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_count_sheet_batches")]
public sealed record class CountSheetBatch : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_count_sheet_batches";
  [Required]
  [Column("batch_key")]
  public override string? Id { get; set; }

  [MaxLength(10, ErrorMessage = "Please batch number must be 10 characters or less")]
  [Column("batch_number")]
  public string? BatchNumber { get; init; }

  [MaxLength(8, ErrorMessage = "Please class of card must be 8 characters or less")]
  [Column("class_of_card")]
  public string? ClassOfCard { get; init; }

  [Column("computer_number_of_records")]
  public short ComputerNumberOfRecords { get; init; }

  [Column("computer_total_amount")]
  public decimal ComputerTotalAmount { get; init; }

  [Column("cost_centre_code")]
  public string? CostCentreCode { get; init; }

  [ForeignKey(nameof(CostCentreCode))]
  public CostCentre? CostCentre { get; set; }
  [NotMapped]
  public string? CostCentre_Caption { get; set; }

  [Column("date")]
  public global::System.DateTime Date { get; init; }

  [MaxLength(10, ErrorMessage = "Please month must be 10 characters or less")]
  [Column("month")]
  public string? Month { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Column("no_of_records")]
  public short NoOfRecords { get; init; }

  [Column("total_amount")]
  public decimal TotalAmount { get; init; }

  public override object ToBaseDTO()
  {
    return (CountSheetBatchDTO)this;
  }
}
