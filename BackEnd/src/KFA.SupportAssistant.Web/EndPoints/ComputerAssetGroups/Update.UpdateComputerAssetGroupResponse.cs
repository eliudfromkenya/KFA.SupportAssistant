namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetGroups;

public class UpdateComputerAssetGroupResponse
{
  public UpdateComputerAssetGroupResponse(ComputerAssetGroupRecord computerAssetGroup)
  {
    ComputerAssetGroup = computerAssetGroup;
  }

  public ComputerAssetGroupRecord ComputerAssetGroup { get; set; }
}
