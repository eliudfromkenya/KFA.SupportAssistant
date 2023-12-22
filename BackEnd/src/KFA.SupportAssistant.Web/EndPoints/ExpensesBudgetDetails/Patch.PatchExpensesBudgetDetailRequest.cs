using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ExpensesBudgetDetails;

public class PatchExpensesBudgetDetailRequest : JsonPatchDocument<ExpensesBudgetDetailDTO>, IPlainTextRequest
{
  public const string Route = "/expenses_budget_details/{expenseBudgetDetailId}";

  public static string BuildRoute(string expenseBudgetDetailId) => Route.Replace("{expenseBudgetDetailId}", expenseBudgetDetailId);

  public string ExpenseBudgetDetailId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ExpensesBudgetDetailDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ExpensesBudgetDetailDTO>>(Content)!;
}
