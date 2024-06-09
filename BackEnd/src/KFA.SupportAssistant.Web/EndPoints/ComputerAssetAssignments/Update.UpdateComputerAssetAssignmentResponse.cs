namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAssignments;

public class UpdateComputerAssetAssignmentResponse
{
  public UpdateComputerAssetAssignmentResponse(ComputerAssetAssignmentRecord computerAssetAssignment)
  {
    ComputerAssetAssignment = computerAssetAssignment;
  }

  public ComputerAssetAssignmentRecord ComputerAssetAssignment { get; set; }
}
