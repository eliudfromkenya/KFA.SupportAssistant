using KFA.SupportAssistant.Core.DataLayer.Types;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAnydesks;

public readonly struct CreateComputerAnydeskResponse(string? anyDeskId, string? anyDeskNumber, string? costCentreCode, string? deviceName, string? nameOfUser, string? narration, string? password, AnyDeskComputerType? type, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AnyDeskId { get; } = anyDeskId;
  public string? AnyDeskNumber { get; } = anyDeskNumber;
  public string? CostCentreCode { get; } = costCentreCode;
  public string? DeviceName { get; } = deviceName;
  public string? NameOfUser { get; } = nameOfUser;
  public string? Narration { get; } = narration;
  public string? Password { get; } = password;
  public AnyDeskComputerType? Type { get; } = type;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
