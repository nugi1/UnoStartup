using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class RoundCardRepository : GenericRepository<RoundCard>, IRoundCardRepository
{
    public RoundCardRepository(UnoContext context) : base(context)
    {
    }
}
