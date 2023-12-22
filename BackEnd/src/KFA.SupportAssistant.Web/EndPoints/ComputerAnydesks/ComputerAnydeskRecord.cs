using KFA.SupportAssistant.Core.DataLayer.Types;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAnydesks;

public record ComputerAnydeskRecord(string? AnyDeskId, string? AnyDeskNumber, string? CostCentreCode, string? DeviceName, string? NameOfUser, string? Narration, string? Password, AnyDeskComputerType? Type, DateTime? DateInserted___, DateTime? DateUpdated___);
