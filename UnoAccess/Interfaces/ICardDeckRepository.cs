using UnoAccess.Entities;

namespace UnoAccess.Interfaces;

public interface ICardDeckRepository
{
    Task FillTheDeck(IList<CardDeck> cards, Guid deckId);

    Task<IEnumerable<CardDeck>> GetCardsByDeck(Guid deckId);

    Task RemoveCardAsync(CardDeck cardDeck);
}