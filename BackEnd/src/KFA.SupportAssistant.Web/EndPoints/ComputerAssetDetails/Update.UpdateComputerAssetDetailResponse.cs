namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetDetails;

public class UpdateComputerAssetDetailResponse
{
  public UpdateComputerAssetDetailResponse(ComputerAssetDetailRecord computerAssetDetail)
  {
    ComputerAssetDetail = computerAssetDetail;
  }

  public ComputerAssetDetailRecord ComputerAssetDetail { get; set; }
}
