namespace KFA.SupportAssistant.Web.EndPoints.LetPropertiesAccounts;

public class GetLetPropertiesAccountByIdRequest
{
  public const string Route = "/let_properties_accounts/{letPropertyAccountId}";

  public static string BuildRoute(string? letPropertyAccountId) => Route.Replace("{letPropertyAccountId}", letPropertyAccountId);

  public string? LetPropertyAccountId { get; set; }
}
