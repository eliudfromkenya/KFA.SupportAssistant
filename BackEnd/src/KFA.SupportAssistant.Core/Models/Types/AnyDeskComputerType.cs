namespace KFA.SupportAssistant.Core.DataLayer.Types;

[Flags]
public enum AnyDeskComputerType : byte
{
  Sales = 1, BackOffice = 2, Manager = 4, Assistant = 8, Other = 16
}
