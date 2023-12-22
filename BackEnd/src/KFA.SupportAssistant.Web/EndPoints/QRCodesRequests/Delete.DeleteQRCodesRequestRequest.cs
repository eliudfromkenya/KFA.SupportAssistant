namespace KFA.SupportAssistant.Web.EndPoints.QRCodesRequests;

public record DeleteQRCodesRequestRequest
{
  public const string Route = "/qr_codes_requests/{qRCodeRequestID}";
  public static string BuildRoute(string? qRCodeRequestID) => Route.Replace("{qRCodeRequestID}", qRCodeRequestID);
  public string? QRCodeRequestID { get; set; }
}
