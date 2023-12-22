namespace KFA.SupportAssistant.Web.EndPoints.QRCodesRequests;

public class GetQRCodesRequestByIdRequest
{
  public const string Route = "/qr_codes_requests/{qRCodeRequestID}";

  public static string BuildRoute(string? qRCodeRequestID) => Route.Replace("{qRCodeRequestID}", qRCodeRequestID);

  public string? QRCodeRequestID { get; set; }
}
