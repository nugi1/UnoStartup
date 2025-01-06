using Shared.DTOs;
using UnoAccess.Entities;

namespace UnoAccess.Interfaces;

public interface ICardRepository
{
    Task<Card?> GetCardByColorAndValue(CardColor color, CardValue value);
    Task<IEnumerable<Card>> GetAllCardsAsync();
}