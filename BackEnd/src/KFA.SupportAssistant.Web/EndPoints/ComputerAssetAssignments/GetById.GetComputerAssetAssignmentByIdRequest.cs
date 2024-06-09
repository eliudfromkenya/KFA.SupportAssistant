namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAssignments;

public class GetComputerAssetAssignmentByIdRequest
{
  public const string Route = "/computer_asset_assignments/{assignmentID}";

  public static string BuildRoute(string? assignmentID) => Route.Replace("{assignmentID}", assignmentID);

  public string? AssignmentID { get; set; }
}
