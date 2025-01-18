using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class PlayerCardRepository(UnoContext context) : IPlayerCardRepository
{
    private DbSet<PlayerCard> DbSet { get; } = context.Set<PlayerCard>();

    private async Task<IList<PlayerCard>> ReadAsync(
        Expression<Func<PlayerCard, bool>>? predicate = null, 
        Func<IQueryable<PlayerCard>, IOrderedQueryable<PlayerCard>>? orderBy = null, 
        Func<IQueryable<PlayerCard>, IIncludableQueryable<PlayerCard, object?>>? include = null, 
        bool noTracking = true)
    {
        IQueryable<PlayerCard> query = DbSet;
        query = noTracking ? query.AsNoTracking() : query;
        query = predicate != null ? query.Where(predicate) : query;
        query = include != null ? include(query) : query;

        return await(orderBy != null ? orderBy(query).ToListAsync() : query.ToListAsync());
    }
    
    public async Task<PlayerCard?> ReadOneAsync(
        Expression<Func<PlayerCard, bool>>? predicate, 
        Func<IQueryable<PlayerCard>, IIncludableQueryable<PlayerCard, object?>>? include = null, 
        bool noTracking = true)
    {
        var query = await ReadAsync(
            predicate: predicate, 
            include: include, 
            noTracking: noTracking);
        var entity = query.FirstOrDefault();

        return entity;
    }


    public async Task<IList<Card>> GetPlayerCards(Guid playerId)
    {
        var playerCards = await DbSet
            .AsNoTracking()
            .Where(pc => pc.PlayerId == playerId)
            .Include(pc => pc.Card)
            .Select(pc => pc.Card)
            .ToListAsync();

        return playerCards;
    }

    public async Task AddPlayerCard(PlayerCard card)
    {
        await DbSet.AddAsync(card);
    }

    public async Task RemovePlayerCard(Guid cardId, Guid playerId)
    {
        var entity = await DbSet.FindAsync(cardId, playerId);

        if (entity != null)
        {
            DbSet.Remove(entity);
        }
    }
}