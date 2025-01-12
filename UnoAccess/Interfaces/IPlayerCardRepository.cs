using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using UnoAccess.Entities;

namespace UnoAccess.Interfaces;

public interface IPlayerCardRepository
{
    Task<IList<Card>> GetPlayerCards(Guid playerId);
    Task AddPlayerCard(PlayerCard card);
    Task RemovePlayerCard(Guid id1, Guid id2);

    Task<PlayerCard> ReadOneAsync(Expression<Func<PlayerCard, bool>>? predicate,
        Func<IQueryable<PlayerCard>, IIncludableQueryable<PlayerCard, object?>>? include = null,
        bool noTracking = true);
}