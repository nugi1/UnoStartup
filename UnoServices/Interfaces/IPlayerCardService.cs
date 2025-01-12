using UnoAccess.Entities;

namespace UnoServices.Interfaces;

public interface IPlayerCardService
{
    Task RemovePlayerCardAsync(Guid cardId, Guid playerId);

    Task AddPlayerCardsAsync(PlayerCard pCard);
    Task GiveCardsToPlayers(IList<Player> players, IList<Guid> cardsInPlay, Guid roundId);
    Task<bool> CardExistsForPlayer(Guid playerId, Guid cardId);
}