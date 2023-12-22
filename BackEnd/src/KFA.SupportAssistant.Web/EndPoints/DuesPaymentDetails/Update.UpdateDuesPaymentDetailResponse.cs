namespace KFA.SupportAssistant.Web.EndPoints.DuesPaymentDetails;

public class UpdateDuesPaymentDetailResponse
{
  public UpdateDuesPaymentDetailResponse(DuesPaymentDetailRecord duesPaymentDetail)
  {
    DuesPaymentDetail = duesPaymentDetail;
  }

  public DuesPaymentDetailRecord DuesPaymentDetail { get; set; }
}
