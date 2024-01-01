using Fluxor;
using KFA.SupportAssistant.Core.DTOs;

namespace KFA.SupportAssistant.RCL.State.MainTitle;

[FeatureState]
public class PaymentsState
{
  public DuesPaymentDetailDTO[]? DuesPayments { get; }
  public bool IsLoading { get; }
  public Exception? Error { get; }

  public PaymentsState(bool isLoading, DuesPaymentDetailDTO[]? duesPayments, Exception? error = null)
  {
    DuesPayments = duesPayments;
    IsLoading = isLoading;
    Error = error;
  }

  public PaymentsState()
  {
  }
}
