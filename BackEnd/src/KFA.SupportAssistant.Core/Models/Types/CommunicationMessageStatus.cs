namespace KFA.SupportAssistant.Core.DataLayer.Types;

[Flags]
public enum CommunicationMessageStatus : byte
{
  Send = 1, Delivered = 2, Received = 4, Read = 8, Undelivered = 16
}
