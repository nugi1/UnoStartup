using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class RoundCardRepository(UnoContext context) : GenericRepository<RoundCard>(context), IRoundCardRepository
{
    public async Task DeleteBy(Guid roundId, Guid cardId)
    {
        var entity = await DbSet.FindAsync(roundId, cardId);

        if (entity != null)
        {
            DbSet.Remove(entity);
        }
    }
    
    public RoundCard? GetTrackedEntity(object roundId, object cardId)
    {
        return context.ChangeTracker.Entries<RoundCard?>()
            .FirstOrDefault(e =>
            {
                var currentValueCardId = e.Property("CardId").CurrentValue;
                var currentValueRoundId = e.Property("RoundId").CurrentValue;
                return currentValueCardId != null && e.Entity != null &&
                       currentValueCardId.Equals(cardId) && currentValueRoundId != null && currentValueRoundId.Equals(roundId);
            })
            ?.Entity;
    }
}
