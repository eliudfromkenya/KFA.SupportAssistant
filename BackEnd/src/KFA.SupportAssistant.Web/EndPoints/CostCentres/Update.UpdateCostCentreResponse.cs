using KFA.SupportAssistant.Web.EndPoints.CostCentres;

namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

public class UpdateCostCentreResponse
{
  public UpdateCostCentreResponse(CostCentreRecord costCentre)
  {
    CostCentre = costCentre;
  }

  public CostCentreRecord CostCentre { get; set; }
}
