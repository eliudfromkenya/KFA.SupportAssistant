using KFA.DynamicsAssistant.Infrastructure.Models;
using KFA.SupportAssistant;

namespace KFA.SupportAssistant.UseCases.DTOs;
public record class TimsMachineDTO : BaseDTO<TimsMachine>
{
  public string? ClassType { get; set; }
  public byte CurrentStatus { get; set; }
  public string? DomainName { get; set; }
  public string? ExternalIPAddress { get; set; }
  public string? ExternalPortNumber { get; set; }
  public string? InternalIPAddress { get; set; }
  public string? InternalPortNumber { get; set; }
  public string? Narration { get; set; }
  public bool ReadyForUse { get; set; }
  public string? SerialNumber { get; set; }
  public string? TimsName { get; set; }
  public override TimsMachine? ToModel()
  {
    return (TimsMachine)this;
  }
  public static implicit operator TimsMachineDTO(TimsMachine obj)
  {
    return new TimsMachineDTO
    {
      ClassType = obj.ClassType,
      CurrentStatus = obj.CurrentStatus,
      DomainName = obj.DomainName,
      ExternalIPAddress = obj.ExternalIPAddress,
      ExternalPortNumber = obj.ExternalPortNumber,
      InternalIPAddress = obj.InternalIPAddress,
      InternalPortNumber = obj.InternalPortNumber,
      Narration = obj.Narration,
      ReadyForUse = obj.ReadyForUse,
      SerialNumber = obj.SerialNumber,
      TimsName = obj.TimsName,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator TimsMachine(TimsMachineDTO obj)
  {
    return new TimsMachine
    {
      ClassType = obj.ClassType,
      CurrentStatus = obj.CurrentStatus,
      DomainName = obj.DomainName,
      ExternalIPAddress = obj.ExternalIPAddress,
      ExternalPortNumber = obj.ExternalPortNumber,
      InternalIPAddress = obj.InternalIPAddress,
      InternalPortNumber = obj.InternalPortNumber,
      Narration = obj.Narration,
      ReadyForUse = obj.ReadyForUse,
      SerialNumber = obj.SerialNumber,
      TimsName = obj.TimsName,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
