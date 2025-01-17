using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class GameRepository(UnoContext context) : GenericRepository<Game>(context), IGameRepository;
