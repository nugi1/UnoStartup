using UnoAccess.Entities;

namespace UnoAccess.Interfaces;

public interface IDiscardPileDeckRepository
{
    Task<IEnumerable<Card>> GetDiscardPilesByDeckId(Guid deckId);
    Task<Card> AddDiscardPileToDeck(Guid deckId);
    Task<Card> GetTopCard();
    Task AddDiscardPile(DiscardPileDeck dp);
    Task RemoveDiscardPiles(Guid deckId);
}