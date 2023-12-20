namespace KFA.SupportAssistant.Core.Models.Types;

[Flags]
public enum IssueStatus : byte
{
  None = 0, Done = 1, Pending = 4, Cancelled = 8, Suspended = 16, ToBeDoneInFuture = 32
}
