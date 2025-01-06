using Shared.DTOs;
using UnoAccess.Entities;

namespace UnoServices.Interfaces;

public interface ICardPlayerService
{
    Task FillThePlayers(Guid deckId);
    Task RemovePlayerCard(CardDto cardDto, Guid deckId, Guid PlayerId);
    Task GiveCardsToPlayer(Guid deckId, Guid playerId, int count);
    Task<IEnumerable<CardPlayer>> GetPlayerCards(Guid playerId);

    Task Clear(IEnumerable<Guid> cardIds);
}