using Shared.DTOs;
using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoServices.Interfaces;

namespace UnoServices.Services;

public class DiscardPileService : IDiscardPileService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DiscardPileService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task AddCardOnTop(Guid deckId, CardDto cardDto)
    {
        var card = await _unitOfWork.CardRepository.GetCardByColorAndValue(cardDto.Color, cardDto.Value);

        await _unitOfWork.DiscardPileDeckRepository.AddDiscardPile(new DiscardPileDeck()
        {
            Added = DateTime.UtcNow,
            CardId = card.Id,
            DeckId = deckId,
        });

        var deck = await _unitOfWork.DeckRepository.GetDeckByIdAsync(deckId);
        deck.TopCardId = card.Id;
        await _unitOfWork.DeckRepository.UpdateDeckAsync(deck);
    }
}