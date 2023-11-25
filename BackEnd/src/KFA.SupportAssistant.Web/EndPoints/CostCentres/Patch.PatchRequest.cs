using FastEndpoints;
using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

public class PatchRequest : JsonPatchDocument<CostCentreDTO>, IPlainTextRequest
{
  public const string Route = "/cost_centres/{id}";
  public string Id { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public JsonPatchDocument<CostCentreDTO> PatchDocument
        => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<CostCentreDTO>>(Content)!;
}
