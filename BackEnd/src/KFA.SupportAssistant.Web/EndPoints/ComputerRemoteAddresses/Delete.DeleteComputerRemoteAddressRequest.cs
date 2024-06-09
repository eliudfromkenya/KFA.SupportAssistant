namespace KFA.SupportAssistant.Web.EndPoints.ComputerRemoteAddresses;

public record DeleteComputerRemoteAddressRequest
{
  public const string Route = "/computer_remote_addresses/{anyDeskId}";
  public static string BuildRoute(string? anyDeskId) => Route.Replace("{anyDeskId}", anyDeskId);
  public string? AnyDeskId { get; set; }
}
