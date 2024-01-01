using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;

namespace KFA.SupportAssistant.Globals.Services;
public interface IHttpClientService
{
  Task DeleteAsync(string id);
  Task<IList<T>> GetAsync<T>(object? body = null);
  Task<T?> GetAsync<T>(ListParam? param = null);
  Task<T?> GetAsync<T>(string? id);
  Task PatchAsync(object obj);
  Task<T?> PostAsync<T>(object obj);
  Task<T?> PutAsync<T>(object obj);

  string? SubURL { get; set; }
  string? AccessToken { get; set; }
}
