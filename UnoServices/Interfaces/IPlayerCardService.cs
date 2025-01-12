using Shared.DTOs.Actions;
using UnoAccess.Entities;

namespace UnoServices.Interfaces;

public interface IPlayerCardService
{
    Task RemovePlayerCardAsync(Guid playerId, PlayCardDto playedCard);

    Task AddPlayerCardsAsync(PlayerCard pCard);
    Task GiveCardsToPlayers(IList<Player> players, IList<Guid> cardsInPlay, Guid roundId);
    Task<bool> CardExistsForPlayer(Guid playerId, Guid cardId);
}