using KFA.DynamicsAssistant.Infrastructure.Models;
using KFA.SupportAssistant;

namespace KFA.SupportAssistant.UseCases.DTOs;
public record class DeviceGuidDTO : BaseDTO<DeviceGuid>
{
  public override DeviceGuid? ToModel()
  {
    return (DeviceGuid)this;
  }
  public static implicit operator DeviceGuidDTO(DeviceGuid obj)
  {
    return new DeviceGuidDTO
    {
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator DeviceGuid(DeviceGuidDTO obj)
  {
    return new DeviceGuid
    {
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
