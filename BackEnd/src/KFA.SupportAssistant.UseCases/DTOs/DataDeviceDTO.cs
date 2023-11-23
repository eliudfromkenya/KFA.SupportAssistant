using KFA.DynamicsAssistant.Infrastructure.Models;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant;

namespace KFA.SupportAssistant.UseCases.DTOs;
public record class DataDeviceDTO : BaseDTO<DataDevice>
{
  public string? DeviceCaption { get; set; }
  public string? DeviceCode { get; set; }
  public string? DeviceName { get; set; }
  public string? DeviceNumber { get; set; }
  public string? DeviceRight { get; set; }
  public string? StationID { get; set; }
  public string? TypeOfDevice { get; set; }
  public override DataDevice? ToModel()
  {
    return (DataDevice)this;
  }
  public static implicit operator DataDeviceDTO(DataDevice obj)
  {
    return new DataDeviceDTO
    {
      DeviceCaption = obj.DeviceCaption,
      DeviceCode = obj.DeviceCode,
      DeviceName = obj.DeviceName,
      DeviceNumber = obj.DeviceNumber,
      DeviceRight = obj.DeviceRight,
      StationID = obj.StationID,
      TypeOfDevice = obj.TypeOfDevice,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator DataDevice(DataDeviceDTO obj)
  {
    return new DataDevice
    {
      DeviceCaption = obj.DeviceCaption,
      DeviceCode = obj.DeviceCode,
      DeviceName = obj.DeviceName,
      DeviceNumber = obj.DeviceNumber,
      DeviceRight = obj.DeviceRight,
      StationID = obj.StationID,
      TypeOfDevice = obj.TypeOfDevice,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
