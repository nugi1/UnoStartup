using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using UnoAccess.Entities;

namespace UnoAccess.Interfaces;

public interface IGenericRepository<T>
        where T : class
{
    Task<T> CreateAsync(T entity);

    Task<T?> ReadByIdAsync(object id);

    Task<T?> ReadOneAsync(
    Expression<Func<T, bool>>? predicate,
    Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null,
    bool noTracking = true);

    Task<IList<T>> ReadAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null,
        bool noTracking = true);
    void Update(T entity);
    Task DeleteAsync(object Id);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> condition);
}
