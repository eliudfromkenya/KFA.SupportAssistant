using Fluxor;
using KFA.SupportAssistant.RCL.State.MainTitle;

namespace KFA.SupportAssistant.RCL.State.Employees;

public static class Reducers
{
  [ReducerMethod]
  public static EmployeesState ReduceMainTitleAction(EmployeesState state, FetchEmployeeDetailsAction titleAction) =>
        new(isLoading: true, employees: [], error: null);

  [ReducerMethod]
  public static PaymentsState ReduceLoggedINUserAction(PaymentsState state, FetchDuePaymentsDetailsAction titleAction) =>
        new(isLoading: true, duesPayments: [], error: null);

  [ReducerMethod]
  public static EmployeesState ReduceMainTitleAction(EmployeesState state, ChangeEmployeeDetailsResultAction titleAction) =>
       new(isLoading: false, employees: titleAction.Employees, error: titleAction.Error);

  [ReducerMethod]
  public static PaymentsState ReduceLoggedINUserAction(PaymentsState state, ChangeDuePaymentsDetailsResultAction titleAction) =>
        new(isLoading: false, duesPayments: titleAction.DuePayments, error: titleAction.Error);
}
