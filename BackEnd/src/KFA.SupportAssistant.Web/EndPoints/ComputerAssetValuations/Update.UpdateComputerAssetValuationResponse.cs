namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetValuations;

public class UpdateComputerAssetValuationResponse
{
  public UpdateComputerAssetValuationResponse(ComputerAssetValuationRecord computerAssetValuation)
  {
    ComputerAssetValuation = computerAssetValuation;
  }

  public ComputerAssetValuationRecord ComputerAssetValuation { get; set; }
}
