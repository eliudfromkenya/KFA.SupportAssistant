namespace KFA.SupportAssistant.Web.EndPoints.ComputerAnydesks;

public class GetComputerAnydeskByIdRequest
{
  public const string Route = "/computer_anydesks/{anyDeskId}";

  public static string BuildRoute(string? anyDeskId) => Route.Replace("{anyDeskId}", anyDeskId);

  public string? AnyDeskId { get; set; }
}
