using KFA.SupportAssistant.Core.ContributorAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KFA.SupportAssistant.Infrastructure.Data;

public static class SeedData{

  public static void Initialize(IServiceProvider serviceProvider)
  {
    using var dbContext = new AppDbContext(
        serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null);
    // Look for any and populate default values.
  }
}
