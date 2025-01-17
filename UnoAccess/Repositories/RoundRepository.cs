using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class RoundRepository(UnoContext context) : GenericRepository<Round>(context), IRoundRepository;
