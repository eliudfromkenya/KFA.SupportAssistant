using Ardalis.Specification;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;

namespace KFA.SupportAssistant.Globals.DataLayer;

public interface IDataRepository<T, Y> where T : BaseDTO<Y>, IBaseDTO where Y : IBaseModel
{
  Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

  Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

  public Task<bool> AnyAsync(Action<ISpecificationBuilder<Y>> query, CancellationToken cancellationToken = default);

  public Task<bool> AnyAsync(CancellationToken cancellationToken = default);

  public IAsyncEnumerable<Y> AsAsyncEnumerable(Action<ISpecificationBuilder<Y>> query);

  public Task<int> CountAsync(Action<ISpecificationBuilder<Y>> query, CancellationToken cancellationToken = default);

  public Task<int> CountAsync(CancellationToken cancellationToken = default);

  public Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

  public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

  public Task<T?> FirstOrDefaultAsync(Action<ISpecificationBuilder<Y>> query, CancellationToken cancellationToken = default);

  public Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<Y, TResult> specification, CancellationToken cancellationToken = default);

  public Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull;

  //public Task<T?> GetBySpecAsync(Action<ISpecificationBuilder<Y>> query, CancellationToken cancellationToken = default);

  //public Task<TResult?> GetBySpecAsync<TResult>(ISpecification<Y, TResult> specification, CancellationToken cancellationToken = default);

  public Task<List<T>> ListAsync(CancellationToken cancellationToken = default);

  public Task<List<T>> ListAsync(Action<ISpecificationBuilder<Y>> query, CancellationToken cancellationToken = default);

  public Task<List<T>> ListAsync(ListParam param, CancellationToken cancellationToken = default);

  public Task<List<TResult>> ListAsync<TResult>(ISpecification<Y, TResult> specification, CancellationToken cancellationToken = default);

  public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

  public Task<T?> SingleOrDefaultAsync(ISingleResultSpecification<Y> specification, CancellationToken cancellationToken = default);

  public Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<Y, TResult> specification, CancellationToken cancellationToken = default);

  public Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

  public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}
