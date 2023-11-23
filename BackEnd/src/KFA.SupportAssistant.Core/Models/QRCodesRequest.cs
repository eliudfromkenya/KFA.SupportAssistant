using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.DynamicsAssistant.Core.DataLayer.Types;
using KFA.SupportAssistant.Globals;

namespace KFA.DynamicsAssistant.Infrastructure.Models;
[Table("tbl_qr_codes_requests")]
public sealed record class QRCodesRequest : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_qr_codes_requests";
  [Column("cost_centre_code")]
  public string? CostCentreCode { get; init; }

  [ForeignKey(nameof(CostCentreCode))]
  public CostCentre? CostCentre { get; set; }

  public string? CostCentre_Caption { get; set; }
  [Required]
  [Column("is_duplicate")]
  public bool IsDuplicate { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Required]
  [Column("qr_code_request_id")]
  public override string? Id { get; set; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please request data must be 255 characters or less")]
  [Column("request_data")]
  public string? RequestData { get; init; }

  [MaxLength(255, ErrorMessage = "Please response data must be 255 characters or less")]
  [Column("response_data")]
  public string? ResponseData { get; init; }

  [MaxLength(255, ErrorMessage = "Please response status must be 255 characters or less")]
  [Column("response_status")]
  public QRResponseType? ResponseStatus { get; init; }
  [Column("time")]
  public global::System.DateTime Time { get; init; }

  [MaxLength(255, ErrorMessage = "Please tims machine used must be 255 characters or less")]
  [Column("tims_machine_used")]
  public string? TimsMachineUsed { get; init; }

  [MaxLength(5, ErrorMessage = "Please vat class must be 5 characters or less")]
  [Column("vat_class")]
  public string? VATClass { get; init; }

  public ICollection<QRRequestItem>? QRRequestItems { get; set; }
}
