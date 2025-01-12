using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class GenericRepository<T> : IGenericRepository<T> 
    where T : class
{
    public DbSet<T> DbSet { get; set; }

    public GenericRepository(UnoContext context)
    {
        DbSet = context.Set<T>();
    }
    public async Task<T> CreateAsync(T entity)
    {
        await DbSet.AddAsync(entity);

        return entity;
    }

    public async Task DeleteAsync(object id)
    {
        var entityToDelete = await DbSet.FindAsync(id);
        if (entityToDelete != null)
        {
            DbSet.Remove(entityToDelete);
        }
    }

    public void DeleteByEntity(T entity)
    {
        DbSet.Remove(entity);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> condition)
    {
        return await DbSet.AnyAsync(condition);
    }

    public async Task<IList<T>> ReadAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, bool noTracking = true)
    {
        IQueryable<T> query = DbSet;
        query = noTracking ? query.AsNoTracking() : query;
        query = predicate != null ? query.Where(predicate) : query;
        query = include != null ? include(query) : query;

        return await(orderBy != null ? orderBy(query).ToListAsync() : query.ToListAsync());
    }

    public async Task<T?> ReadByIdAsync(object id)
    {
        var entity = await DbSet.FindAsync(id);

        return entity;
    }

    public async Task<T?> ReadOneAsync(Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, bool noTracking = true)
    {
        var query = await ReadAsync(
            predicate: predicate, 
            include: include, 
            noTracking: noTracking);
        var entity = query.FirstOrDefault();

        return entity;
    }

    public void Update(T entity)
    {
        if (DbSet.Entry(entity).State == EntityState.Detached)
        {
            DbSet.Attach(entity);
        }

        DbSet.Entry(entity).State = EntityState.Modified;
    }
}
