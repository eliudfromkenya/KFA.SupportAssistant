using Ardalis.HttpClientTestExtensions;
using KFA.SupportAssistant.Infrastructure.Data;
using KFA.SupportAssistant.Web.ContributorEndpoints;
using KFA.SupportAssistant.Web.Endpoints.ContributorEndpoints;
using Xunit;

namespace KFA.SupportAssistant.FunctionalTests.ApiEndpoints;

[Collection("Sequential")]
public class ContributorGetById : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public ContributorGetById(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsSeedContributorGivenId1()
  {
    var result = await _client.GetAndDeserializeAsync<ContributorRecord>(GetContributorByIdRequest.BuildRoute("AAA-01"));

    Assert.Equal("AAA-01", result.Id);
    Assert.Equal(SeedData.Contributor1.Name, result.Name);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenId1000()
  {
    string route = GetContributorByIdRequest.BuildRoute("AAC-01");
    _ = await _client.GetAndEnsureNotFoundAsync(route);
  }
}
