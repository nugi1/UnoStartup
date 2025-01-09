using Shared.DTOs;
using UnoAccess.Entities;

namespace UnoServices.Interfaces;

public interface IDeckService
{
    Task<Deck> CreateDeck(CreateDeckDto deckDto);
    Task<Deck?> ReadDeck(Guid id);
    Task<Player> GetPlayerWithTurn(Guid deckId);
    Task<Player> SetTurn(int position, Guid deckId);
    Task TogglePlayDirection(Guid deckId);
    Task<Player> SetScoreAsync(Guid playerId, Guid deckId, int score);
    Task RefreshDeck(Guid deckId);

    Task UpdateDeck(Deck deck);
}