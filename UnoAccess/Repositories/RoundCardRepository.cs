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
}
