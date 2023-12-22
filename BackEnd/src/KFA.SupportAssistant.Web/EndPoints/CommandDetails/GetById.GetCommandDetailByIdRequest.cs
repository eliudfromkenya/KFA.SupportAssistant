namespace KFA.SupportAssistant.Web.EndPoints.CommandDetails;

public class GetCommandDetailByIdRequest
{
  public const string Route = "/command_details/{commandId}";

  public static string BuildRoute(string? commandId) => Route.Replace("{commandId}", commandId);

  public string? CommandId { get; set; }
}
