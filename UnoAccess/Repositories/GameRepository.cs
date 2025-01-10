using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class GameRepository : GenericRepository<Game>, IGameRepository
{
    public GameRepository(UnoContext context) : base(context)
    {
    }
}
