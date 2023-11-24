namespace KFA.SupportAssistant.Core.DataLayer.Types;

[Flags]
public enum QRResponseType : byte
{
  Recieved = 1, SuccessfullyRespondedTo = 2, IsDuplicated = 4
}
