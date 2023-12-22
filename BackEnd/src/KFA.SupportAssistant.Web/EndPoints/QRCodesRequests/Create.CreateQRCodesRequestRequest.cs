using System.ComponentModel.DataAnnotations;
using KFA.SupportAssistant.Core.DataLayer.Types;

namespace KFA.SupportAssistant.Web.EndPoints.QRCodesRequests;

public class CreateQRCodesRequestRequest
{
  public const string Route = "/qr_codes_requests";
  public string? CostCentreCode { get; set; }

  [Required]
  public bool? IsDuplicate { get; set; }

  public string? Narration { get; set; }

  [Required]
  public string? QRCodeRequestID { get; set; }

  [Required]
  public string? RequestData { get; set; }

  public string? ResponseData { get; set; }
  public QRResponseType? ResponseStatus { get; set; }
  public global::System.DateTime? Time { get; set; }
  public string? TimsMachineused { get; set; }
  public string? VATClass { get; set; }
}
