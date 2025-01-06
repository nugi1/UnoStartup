using UnoAccess.Entities;

namespace UnoServices.Interfaces;

public interface ICardService
{
    Task<Card> GetCard(Guid cardId);
}