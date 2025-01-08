using System.Collections;
using UnoAccess.Entities;

namespace UnoServices.Interfaces;

public interface ICardDeckService
{ 
    Task FillTheDeck(Guid deckId);
    Task RemoveCardOnTop(Guid deckId);
    Task Clear(Guid deckId);

    Task<CardDeck> GetTopCard(Guid deckId);
    
    Task Shuffle(Guid deckId);
}