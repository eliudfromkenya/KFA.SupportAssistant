using KFA.SupportAssistant.Core.Models;
namespace KFA.SupportAssistant.Core.DTOs;
public record class ComputerRemoteAddressDTO : BaseDTO<ComputerRemoteAddress>
{
        public string? AnyDeskNumber { get; set; }
        public string? AnydeskPassword { get; set; }
        public string? AssetDetailID { get; set; }
        public string? CostCentreCode { get; set; }
        public string? DeviceName { get; set; }
        public string? NameOfUser { get; set; }
        public string? Narration { get; set; }
        public string? TeamViewerAddress { get; set; }
        public string? Type { get; set; }
public override ComputerRemoteAddress? ToModel()
  {
    return (ComputerRemoteAddress)this;
  }


  public static implicit operator ComputerRemoteAddressDTO(ComputerRemoteAddress obj)
  {
    return new ComputerRemoteAddressDTO
    {
      AnyDeskNumber = obj.AnyDeskNumber,
      AnydeskPassword = obj.AnydeskPassword,
      AssetDetailID = obj.AssetDetailID,
      CostCentreCode = obj.CostCentreCode,
      DeviceName = obj.DeviceName,
      NameOfUser = obj.NameOfUser,
      Narration = obj.Narration,
      TeamViewerAddress = obj.TeamViewerAddress,
      Type = obj.Type,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ComputerRemoteAddress(ComputerRemoteAddressDTO obj)
  {
    return new ComputerRemoteAddress
    {
      AnyDeskNumber = obj.AnyDeskNumber,
      AnydeskPassword = obj.AnydeskPassword,
      AssetDetailID = obj.AssetDetailID,
      CostCentreCode = obj.CostCentreCode,
      DeviceName = obj.DeviceName,
      NameOfUser = obj.NameOfUser,
      Narration = obj.Narration,
      TeamViewerAddress = obj.TeamViewerAddress,
      Type = obj.Type,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }

}
