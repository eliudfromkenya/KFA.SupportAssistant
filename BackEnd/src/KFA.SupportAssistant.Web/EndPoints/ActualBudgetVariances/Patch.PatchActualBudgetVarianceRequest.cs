using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariances;

public class PatchActualBudgetVarianceRequest : JsonPatchDocument<ActualBudgetVarianceDTO>, IPlainTextRequest
{
  public const string Route = "/actual_budget_variances/{actualBudgetID}";

  public static string BuildRoute(string actualBudgetID) => Route.Replace("{actualBudgetID}", actualBudgetID);

  public string ActualBudgetID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ActualBudgetVarianceDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ActualBudgetVarianceDTO>>(Content)!;
}
