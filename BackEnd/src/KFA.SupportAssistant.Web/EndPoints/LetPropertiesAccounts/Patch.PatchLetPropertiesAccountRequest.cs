using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.LetPropertiesAccounts;

public class PatchLetPropertiesAccountRequest : JsonPatchDocument<LetPropertiesAccountDTO>, IPlainTextRequest
{
  public const string Route = "/let_properties_accounts/{letPropertyAccountId}";

  public static string BuildRoute(string letPropertyAccountId) => Route.Replace("{letPropertyAccountId}", letPropertyAccountId);

  public string LetPropertyAccountId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<LetPropertiesAccountDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<LetPropertiesAccountDTO>>(Content)!;
}
