using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class RoundCardRepository : GenericRepository<RoundCard>, IRoundCardRepository
{
    public RoundCardRepository(UnoContext context) : base(context)
    {
    }

    public async Task DeleteBy(Guid id1, Guid id2)
    {
        var entity = await DbSet.FindAsync(id1, id2);

        if (entity != null)
        {
            DbSet.Remove(entity);
        }
    }
}
