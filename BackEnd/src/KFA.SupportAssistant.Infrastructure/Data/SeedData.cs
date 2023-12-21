using KFA.SupportAssistant.Core.Classes;
using KFA.SupportAssistant.Core.ContributorAggregate;
using KFA.SupportAssistant.Infrastructure.Data.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KFA.SupportAssistant.Infrastructure.Data;

public static class SeedData{

  public static void Initialize(IServiceProvider serviceProvider)
  {
    using var dbContext = new AppDbContext(
        serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null);

    AsyncUtil.RunSync(() => PayrollGroups.Process(dbContext));
    // Look for any and populate default values.
  }
}
