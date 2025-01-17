using Shared.DTOs.Actions;
using UnoAccess.Entities;

namespace UnoServices.Interfaces;

public interface IPlayerCardService
{
    Task RemovePlayerCardAsync(Guid playerId, PlayCardDto playedCard);

    Task AddPlayerCardsAsync(PlayerCard pCard);
    Task GiveCardsToPlayers(IList<Player> players, IList<Guid> cardsInPlay, Guid roundId);
    Task<bool> CardExistsForPlayer(Guid playerId, Guid cardId);
    Task GiveCardToPlayer(Guid playerId, Guid cardId);
    Task GivePlayerTwoCards(Guid playerId, Guid roundId);
    Task<int> GetCardsQuantity(Guid playerId);
    Task<int> AccumulateScoresAsync(Guid? gameId, Guid playerId);
    Task DeletePlayerCardsAsync(IEnumerable<Guid> playerIds);
    Task<IEnumerable<Guid>> GetCardsInPlayAsync(Guid? actionGameId);
    Task<Card> GetCardFromPlayersHand(Guid id, PlayCardDto playedCard);
}