namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

public class UpdateCostCentreResponse
{
  public UpdateCostCentreResponse(CostCentreRecord costCentre)
  {
    CostCentre = costCentre;
  }

  public CostCentreRecord CostCentre { get; set; }
}
