using KFA.SupportAssistant.Web.ContributorEndpoints;

namespace KFA.SupportAssistant.Web.Endpoints.ContributorEndpoints;

public class UpdateContributorResponse
{
  public UpdateContributorResponse(ContributorRecord contributor)
  {
    Contributor = contributor;
  }

  public ContributorRecord Contributor { get; set; }
}
