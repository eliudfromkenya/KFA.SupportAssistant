using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetDetails;

public class PatchPurchasesBudgetDetailRequest : JsonPatchDocument<PurchasesBudgetDetailDTO>, IPlainTextRequest
{
  public const string Route = "/purchases_budget_details/{purchasesBudgetDetailId}";

  public static string BuildRoute(string purchasesBudgetDetailId) => Route.Replace("{purchasesBudgetDetailId}", purchasesBudgetDetailId);

  public string PurchasesBudgetDetailId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<PurchasesBudgetDetailDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<PurchasesBudgetDetailDTO>>(Content)!;
}
