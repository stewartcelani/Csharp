using System.Linq.Expressions;
using CityInfo.API.Domain.Entities.Common;

namespace CityInfo.API.Repositories.Common;

public interface IRepository<TEntity, in TKey>
    where TEntity : BaseEntity<TKey>
    where TKey : IEquatable<TKey>
{
    Task<TEntity?> GetAsync(TKey id, IEnumerable<string>? includeProperties = null);

    Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        IEnumerable<string>? includeProperties = null);

    Task<bool> ExistsAsync(TKey id);

    Task<bool> CreateAsync(TEntity entity);
    Task<bool> CreateAsync(IEnumerable<TEntity> entities);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> UpdateAsync(IEnumerable<TEntity> entities);
    Task<bool> DeleteAsync(TEntity entity);
    Task<bool> DeleteAsync(IEnumerable<TEntity> entities);
}