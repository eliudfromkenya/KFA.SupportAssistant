using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetDetails;

public class PatchSalesBudgetDetailRequest : JsonPatchDocument<SalesBudgetDetailDTO>, IPlainTextRequest
{
  public const string Route = "/sales_budget_details/{salesBudgetDetailId}";

  public static string BuildRoute(string salesBudgetDetailId) => Route.Replace("{salesBudgetDetailId}", salesBudgetDetailId);

  public string SalesBudgetDetailId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<SalesBudgetDetailDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<SalesBudgetDetailDTO>>(Content)!;
}
