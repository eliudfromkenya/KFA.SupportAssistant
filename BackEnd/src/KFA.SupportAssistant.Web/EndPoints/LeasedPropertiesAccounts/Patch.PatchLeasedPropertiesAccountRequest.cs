using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.LeasedPropertiesAccounts;

public class PatchLeasedPropertiesAccountRequest : JsonPatchDocument<LeasedPropertiesAccountDTO>, IPlainTextRequest
{
  public const string Route = "/leased_properties_accounts/{leasedPropertyAccountId}";

  public static string BuildRoute(string leasedPropertyAccountId) => Route.Replace("{leasedPropertyAccountId}", leasedPropertyAccountId);

  public string LeasedPropertyAccountId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<LeasedPropertiesAccountDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<LeasedPropertiesAccountDTO>>(Content)!;
}
