using Fluxor;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.Globals.Services;
using KFA.SupportAssistant.RCL.Models.Data;
using KFA.SupportAssistant.RCL.Services;
using KFA.SupportAssistant.RCL.State.MainTitle;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;

namespace KFA.SupportAssistant.RCL.State.DuePayments.Effects;

public class FetchDuePaymentsEffect
{
  readonly IHttpClientService _httpClient;
  public FetchDuePaymentsEffect(IHttpClientService httpClient)
  {
    _httpClient = httpClient;
    _httpClient.SubURL = "employee-details";
  }
  [EffectMethod]
  public async Task HandleFetchDataAction(FetchDuePaymentsDetailsAction action, IDispatcher dispatcher)
  {
    await Task.Run(async () =>
    {
      try
      {
        Thread.Sleep(3000);
        var elements = await _httpClient.GetAsync<DuesPaymentDetailDTO>(new ListParam { Skip = 0, Take = 1000000 });
       // dispatcher.Dispatch(new ChangeDuePaymentsDetailsResultAction { Error = null, DuePayments = elements.ToArray() ?? [] });
      }
      catch (Exception ex)
      {
        dispatcher.Dispatch(new ChangeDuePaymentsDetailsResultAction { Error = ex, DuePayments = [] });
      }
    });
  }
}
