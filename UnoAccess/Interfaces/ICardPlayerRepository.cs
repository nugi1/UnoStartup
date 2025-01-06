using UnoAccess.Entities;

namespace UnoAccess.Interfaces;

public interface ICardPlayerRepository
{
    Task GiveCardsToPlayers(Guid deckId, List<Guid> cards, List<Guid> playerIds);
    
    Task RemoveAsync(CardPlayer cardPlayer);

    Task GiveCardToPlayer(Guid deckId, Guid playerId, List<Guid> cardIds);

    Task<CardPlayer?> GetCardPlayer(Guid playerId, Guid cardId);
    Task<IEnumerable<CardPlayer>> GetPlayerCards(Guid playerId);
}