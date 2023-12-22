namespace KFA.SupportAssistant.Web.EndPoints.LeasedPropertiesAccounts;

public class GetLeasedPropertiesAccountByIdRequest
{
  public const string Route = "/leased_properties_accounts/{leasedPropertyAccountId}";

  public static string BuildRoute(string? leasedPropertyAccountId) => Route.Replace("{leasedPropertyAccountId}", leasedPropertyAccountId);

  public string? LeasedPropertyAccountId { get; set; }
}
