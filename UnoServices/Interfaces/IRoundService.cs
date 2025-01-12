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
}