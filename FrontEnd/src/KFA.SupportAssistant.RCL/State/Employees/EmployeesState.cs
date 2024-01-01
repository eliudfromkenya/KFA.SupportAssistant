using Fluxor;
using KFA.SupportAssistant.RCL.Models.Data;

namespace KFA.SupportAssistant.RCL.State.Employees;

[FeatureState]
public class EmployeesState
{
  public EmployeeDetailDTO[]? Employees { get; }
  public Exception? Error { get; }  
  public bool IsLoading { get; }

  public EmployeesState(bool isLoading, EmployeeDetailDTO[]? employees, Exception? error = null)
  {
    Employees = employees ?? [];
    IsLoading = isLoading;
    Error = error;
  }

  public EmployeesState()
  {
  }
}
