using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class CardRepository : GenericRepository<Card>, ICardRepository
{
    public CardRepository(UnoContext context) : base(context)
    {
    }
}
