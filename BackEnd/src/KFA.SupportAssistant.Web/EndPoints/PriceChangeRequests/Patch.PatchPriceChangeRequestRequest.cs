using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.PriceChangeRequests;

public class PatchPriceChangeRequestRequest : JsonPatchDocument<PriceChangeRequestDTO>, IPlainTextRequest
{
  public const string Route = "/price_change_requests/{requestID}";

  public static string BuildRoute(string requestID) => Route.Replace("{requestID}", requestID);

  public string RequestID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<PriceChangeRequestDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<PriceChangeRequestDTO>>(Content)!;
}
