using KFA.SupportAssistant.Core.Services;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Infrastructure.Data;

internal class DbQuery<T>(AppDbContext dbContext) : IDbQuery<T> where T : BaseModel, new()
{
  public IQueryable<T> GetQuery()
  {
    return dbContext.Set<T>().AsQueryable();
  }
}
