using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class PlayerRepository(UnoContext context) : GenericRepository<Player>(context), IPlayerRepository;
