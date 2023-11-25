using KFA.SupportAssistant.Core.DataLayer.Types;
using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class QRCodesRequestDTO : BaseDTO<QRCodesRequest>
{
  public string? CostCentreCode { get; set; }
  public bool IsDuplicate { get; set; }
  public string? Narration { get; set; }
  public string? RequestData { get; set; }
  public string? ResponseData { get; set; }
  public QRResponseType? ResponseStatus { get; set; }
  public DateTime Time { get; set; }
  public string? TimsMachineUsed { get; set; }
  public string? VATClass { get; set; }
  public override QRCodesRequest? ToModel()
  {
    return (QRCodesRequest)this;
  }
  public static implicit operator QRCodesRequestDTO(QRCodesRequest obj)
  {
    return new QRCodesRequestDTO
    {
      CostCentreCode = obj.CostCentreCode,
      IsDuplicate = obj.IsDuplicate,
      Narration = obj.Narration,
      RequestData = obj.RequestData,
      ResponseData = obj.ResponseData,
      ResponseStatus = obj.ResponseStatus,
      Time = obj.Time,
      TimsMachineUsed = obj.TimsMachineUsed,
      VATClass = obj.VATClass,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator QRCodesRequest(QRCodesRequestDTO obj)
  {
    return new QRCodesRequest
    {
      CostCentreCode = obj.CostCentreCode,
      IsDuplicate = obj.IsDuplicate,
      Narration = obj.Narration,
      RequestData = obj.RequestData,
      ResponseData = obj.ResponseData,
      ResponseStatus = obj.ResponseStatus,
      Time = obj.Time,
      TimsMachineUsed = obj.TimsMachineUsed,
      VATClass = obj.VATClass,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
