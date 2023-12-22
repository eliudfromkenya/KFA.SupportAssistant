using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.QRCodesRequests;

public record UpdateQRCodesRequestRequest
{
  public const string Route = "/qr_codes_requests/{qRCodeRequestID}";
  public string? CostCentreCode { get; set; }
  [Required]
  public bool? IsDuplicate { get; set; }
  public string? Narration { get; set; }
  [Required]
  public string? QRCodeRequestID { get; set; }
  [Required]
  public string? RequestData { get; set; }
  public string? ResponseData { get; set; }
  public string? ResponseStatus { get; set; }
  public global::System.DateTime? Time { get; set; }
  public string? TimsMachineused { get; set; }
  public string? VATClass { get; set; }
}
