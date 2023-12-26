namespace KFA.SupportAssistant.RCL.Services
{
  internal interface IBookStoresService<T>
  {
    Task<List<T>> GetAllAsync(string requestUri);

    Task<T> GetByIdAsync(string requestUri, int Id);

    Task<T> SaveAsync(string requestUri, T obj);

    Task<T> UpdateAsync(string requestUri, int Id, T obj);

    Task<bool> DeleteAsync(string requestUri, int Id);
  }
}
