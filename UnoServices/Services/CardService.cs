using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoServices.Interfaces;

namespace UnoServices.Services;

public class CardService : ICardService
{
    private readonly IUnitOfWork _unitOfWork;
    public CardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<Card>> GetAllCards()
    {
        return await _unitOfWork.CardRepository.GetAllCardsAsync();
    }

    public async Task<Card> GetCard(Guid cardId)
    {
        using (var db = new UnoContext())
        {
            return await db.Cards.FindAsync(cardId);
        }
    }
}