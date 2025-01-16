using Shared.DTOs.Actions;
using Shared.Enums;
using UnoAccess.Entities;

namespace UnoServices.Interfaces;

public interface IRoundService
{
    Task UpdateRoundTopCardAsync(Guid? roundId, Card topCard);
    Task RemoveCardInRoundAsync(Guid roundId, Guid cardId);
    Task FillTheCardsInPlayAsync(Guid roundId);
    Task<IList<RoundCard>> GetCardsInRound(Guid roundId);
    Task CreateRound(Round round);
    Task<Card> ShuffleUntilTopCardAsync(Guid roundId);
    Task DeleteCardsInPlayAsync(Guid roundId);
    Task ReshuffleDiscardPilesAsync(Guid roundId, IEnumerable<Guid> playerCardIds);
    Task PlayedWildCardAsync(Guid roundId, PlayCardDto playedCard);
    Task<CardColor?> CheckForWildTopCardAsync(Guid roundId);
    Task RemoveTopCardWildColor(Guid roundId);
}