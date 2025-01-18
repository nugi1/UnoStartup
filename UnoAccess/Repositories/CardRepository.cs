using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class CardRepository(UnoContext context) : GenericRepository<Card>(context), ICardRepository;
