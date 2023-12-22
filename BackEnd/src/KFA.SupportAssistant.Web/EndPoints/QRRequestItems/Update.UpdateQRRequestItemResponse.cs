namespace KFA.SupportAssistant.Web.EndPoints.QRRequestItems;

public class UpdateQRRequestItemResponse
{
  public UpdateQRRequestItemResponse(QRRequestItemRecord qRRequestItem)
  {
    QRRequestItem = qRRequestItem;
  }

  public QRRequestItemRecord QRRequestItem { get; set; }
}
