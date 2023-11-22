using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.Endpoints.ContributorEndpoints;

public class UpdateContributorRequest
{
  public const string Route = "/Contributors/{ContributorId}";
  public static string BuildRoute(string contributorId) => Route.Replace("{ContributorId}", contributorId.ToString());

  public string? ContributorId { get; set; }

  [Required]
  public string? Id { get; set; }
  [Required]
  public string? Name { get; set; }
}
