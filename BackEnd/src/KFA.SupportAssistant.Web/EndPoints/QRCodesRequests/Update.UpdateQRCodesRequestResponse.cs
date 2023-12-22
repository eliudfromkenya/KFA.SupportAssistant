namespace KFA.SupportAssistant.Web.EndPoints.QRCodesRequests;

public class UpdateQRCodesRequestResponse
{
  public UpdateQRCodesRequestResponse(QRCodesRequestRecord qRCodesRequest)
  {
    QRCodesRequest = qRCodesRequest;
  }

  public QRCodesRequestRecord QRCodesRequest { get; set; }
}
