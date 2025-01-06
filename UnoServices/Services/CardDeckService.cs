using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoServices.Interfaces;

namespace UnoServices.Services;

public class CardDeckService : ICardDeckService
{
    private readonly ICardDeckRepository _cardDeckRepository;
    private readonly ICardRepository _cardRepository;
    public CardDeckService(ICardDeckRepository cardDeckRepository, ICardRepository cardRepository)
    {
        _cardDeckRepository = cardDeckRepository;
        _cardRepository = cardRepository;
    }
    public async Task FillTheDeck(Guid deckId)
    {
        var cards = await _cardRepository.GetAllCardsAsync();
        await _cardDeckRepository.FillTheDeck(cards.Select(c => new CardDeck()
        {
            DeckId = deckId,
            CardId = c.Id
        }).ToList(), deckId);
    }

    public async Task RemoveCardOnTop(Guid deckId)
    {
        var cards = await _cardDeckRepository.GetCardsByDeck(deckId);
        var card = cards.OrderBy(c => c.Position).First();
        await _cardDeckRepository.RemoveCardAsync(card);
    }

    public async Task Clear(Guid deckId)
    {
        var cards = await _cardDeckRepository.GetCardsByDeck(deckId);

        foreach (var card in cards)
        {
            await _cardDeckRepository.RemoveCardAsync(new CardDeck()
            {
                CardId = card.CardId,
                DeckId = deckId,
            });
        }
    }
}