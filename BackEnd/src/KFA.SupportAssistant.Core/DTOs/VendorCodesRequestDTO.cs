using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class VendorCodesRequestDTO : BaseDTO<VendorCodesRequest>
{
  public string? AttandedBy { get; set; }
  public string? CostCentreCode { get; set; }
  public string? Description { get; set; }
  public string? Narration { get; set; }
  public string? RequestingUser { get; set; }
  public string? Status { get; set; }
  public string? TimeAttended { get; set; }
  public DateTime? TimeOfRequest { get; set; }
  public string? VendorCode { get; set; }
  public string? VendorType { get; set; }
  public override VendorCodesRequest? ToModel()
  {
    return (VendorCodesRequest)this;
  }

  public static implicit operator VendorCodesRequestDTO(VendorCodesRequest obj)
  {
    return new VendorCodesRequestDTO
    {
      AttandedBy = obj.AttandedBy,
      CostCentreCode = obj.CostCentreCode,
      Description = obj.Description,
      Narration = obj.Narration,
      RequestingUser = obj.RequestingUser,
      Status = obj.Status,
      TimeAttended = obj.TimeAttended,
      TimeOfRequest = obj.TimeOfRequest,
      VendorCode = obj.VendorCode,
      VendorType = obj.VendorType,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator VendorCodesRequest(VendorCodesRequestDTO obj)
  {
    return new VendorCodesRequest
    {
      AttandedBy = obj.AttandedBy,
      CostCentreCode = obj.CostCentreCode,
      Description = obj.Description,
      Narration = obj.Narration,
      RequestingUser = obj.RequestingUser,
      Status = obj.Status,
      TimeAttended = obj.TimeAttended,
      TimeOfRequest = obj.TimeOfRequest,
      VendorCode = obj.VendorCode,
      VendorType = obj.VendorType,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
