using UnoAccess.Entities;

namespace UnoAccess.Interfaces;

public interface IDeckRepository
{
    Task<Deck?> GetDeckByIdAsync(Guid deckId);
    Task<IEnumerable<Deck>> GetAllDecksAsync();
    Task<Deck> CreateDeckAsync(Deck deck);
    Task<Player> GetTurn(Guid deckId);
    Task<Card> GetTopCard(Guid deckId);
    Task<Deck> UpdateDeckAsync(Deck deck);
    Task DeleteDeckAsync(Guid deckId);
}