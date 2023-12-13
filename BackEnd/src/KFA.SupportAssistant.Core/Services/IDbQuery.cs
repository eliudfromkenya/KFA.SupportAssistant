using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Services;

public interface IDbQuery<T> where T : BaseModel, new()
{
  IQueryable<T> GetQuery();
}
