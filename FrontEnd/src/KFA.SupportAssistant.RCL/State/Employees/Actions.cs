using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.RCL.Models.Data;

namespace KFA.SupportAssistant.RCL.State.MainTitle;

public readonly struct FetchDuePaymentsDetailsAction
{
}

public readonly struct ChangeDuePaymentsDetailsResultAction
{
  public DuesPaymentDetailDTO[]? DuePayments { get; init; }

  public Exception? Error
  {
    get; init;
  }
}

public readonly struct FetchEmployeeDetailsAction
{
}

public readonly struct ChangeEmployeeDetailsResultAction
{
  public EmployeeDetailDTO[]? Employees { get; init; }
  public Exception? Error { get; init; }
}
