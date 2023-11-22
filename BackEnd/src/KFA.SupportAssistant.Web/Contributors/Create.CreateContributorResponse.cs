namespace KFA.SupportAssistant.Web.Endpoints.ContributorEndpoints;

public class CreateContributorResponse
{
  public CreateContributorResponse(string id, string name)
  {
    Id = id;
    Name = name;
  }
  public string Id { get; set; }
  public string Name { get; set; }
}
