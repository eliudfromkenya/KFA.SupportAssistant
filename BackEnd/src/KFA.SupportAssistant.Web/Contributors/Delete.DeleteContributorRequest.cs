namespace KFA.SupportAssistant.Web.Endpoints.ContributorEndpoints;

public record DeleteContributorRequest
{
  public const string Route = "/Contributors/{ContributorId}";
  public static string BuildRoute(string contributorId) => Route.Replace("{ContributorId}", contributorId.ToString());

  public string ContributorId { get; set; } = string.Empty;
}
