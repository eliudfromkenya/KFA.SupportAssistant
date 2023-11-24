using KFA.SupportAssistant.Core.DataLayer.Types;
using KFA.SupportAssistant.Infrastructure.Models;
using KFA.SupportAssistant;

namespace KFA.SupportAssistant.UseCases.DTOs;
public record class ComputerAnydeskDTO : BaseDTO<ComputerAnydesk>
{
  public string? AnyDeskNumber { get; set; }
  public string? CostCentreCode { get; set; }
  public string? DeviceName { get; set; }
  public string? Narration { get; set; }
  public AnyDeskComputerType? Type { get; set; }
  public string? NameOfUser { get; set; }
  public string? Password { get; set; }

  public override ComputerAnydesk? ToModel()
  {
    return (ComputerAnydesk)this;
  }
  public static implicit operator ComputerAnydeskDTO(ComputerAnydesk obj)
  {
    return new ComputerAnydeskDTO
    {
      AnyDeskNumber = obj.AnyDeskNumber,
      CostCentreCode = obj.CostCentreCode,
      DeviceName = obj.DeviceName,
      Narration = obj.Narration,
      NameOfUser = obj.NameOfUser,
      Password = obj.Password,
      Type = obj.Type,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ComputerAnydesk(ComputerAnydeskDTO obj)
  {
    return new ComputerAnydesk
    {
      AnyDeskNumber = obj.AnyDeskNumber,
      CostCentreCode = obj.CostCentreCode,
      DeviceName = obj.DeviceName,
      Narration = obj.Narration,
      NameOfUser = obj.NameOfUser,
      Password = obj.Password,
      Type = obj.Type,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
