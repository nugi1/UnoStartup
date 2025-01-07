using UnoAccess.Entities;

namespace UnoServices.Interfaces;

public interface ICardService
{
    Task<Card> GetCard(Guid cardId);

    Task<IList<Card>> GetCards(IEnumerable<Guid> cardIds);
}