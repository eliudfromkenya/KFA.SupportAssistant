using KFA.SupportAssistant.Core.DataLayer.Types;

namespace KFA.SupportAssistant.Web.EndPoints.QRCodesRequests;

public readonly struct CreateQRCodesRequestResponse(string? costCentreCode, bool? isDuplicate, string? narration, string? qRCodeRequestID, string? requestData, string? responseData, QRResponseType? responseStatus, global::System.DateTime? time, string? timsMachineUsed, string? vATClass, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? CostCentreCode { get; } = costCentreCode;
  public bool? IsDuplicate { get; } = isDuplicate;
  public string? Narration { get; } = narration;
  public string? QRCodeRequestID { get; } = qRCodeRequestID;
  public string? RequestData { get; } = requestData;
  public string? ResponseData { get; } = responseData;
  public QRResponseType? ResponseStatus { get; } = responseStatus;
  public global::System.DateTime? Time { get; } = time;
  public string? TimsMachineused { get; } = timsMachineUsed;
  public string? VATClass { get; } = vATClass;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
