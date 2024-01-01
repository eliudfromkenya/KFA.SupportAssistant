using Fluxor;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.Globals.Services;
using KFA.SupportAssistant.RCL.Models.Data;
using KFA.SupportAssistant.RCL.Services;
using KFA.SupportAssistant.RCL.State.MainTitle;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;

namespace KFA.SupportAssistant.RCL.State.Employees.Effects;

public class FetchEmployeesEffect(IHttpClientService httpClient, IState<LoggedInUserState> userState)
{
  [EffectMethod]
  public async Task HandleFetchDataAction(FetchEmployeeDetailsAction action, IDispatcher dispatcher)
  {
    await Task.Run(async () =>
    {
      try
      {
        httpClient.SubURL = "employee_details";
        var token = userState.Value?.User?.Token;
        if (!string.IsNullOrWhiteSpace(token))
          httpClient.AccessToken = token;

        var elements = await httpClient.GetAsync<Result>(new ListParam { Skip = 0, Take = 1000000 });
        dispatcher.Dispatch(new ChangeEmployeeDetailsResultAction { Error = null, Employees = elements?.EmployeeDetails?.ToArray() ?? [] });
      }
      catch (Exception ex)
      {
        dispatcher.Dispatch(new ChangeEmployeeDetailsResultAction { Error = ex, Employees = [] });
      }
    });
  }
  public record Result { public EmployeeDetailDTO[]? EmployeeDetails { get; set;} }
}
