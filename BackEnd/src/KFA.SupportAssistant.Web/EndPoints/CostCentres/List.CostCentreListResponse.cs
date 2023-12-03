using KFA.SupportAssistant.Web.EndPoints.CostCentres;

namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

public class CostCentreListResponse
{
  public List<CostCentreRecord> CostCentres { get; set; } = [];
}
