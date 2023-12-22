namespace KFA.SupportAssistant.Web.EndPoints.DuesPaymentDetails;

public record DeleteDuesPaymentDetailRequest
{
  public const string Route = "/dues_payment_details/{paymentID}";
  public static string BuildRoute(string? paymentID) => Route.Replace("{paymentID}", paymentID);
  public string? PaymentID { get; set; }
}
