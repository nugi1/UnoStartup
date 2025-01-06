namespace UnoServices.Interfaces;

public interface ICardDeckService
{ 
    Task FillTheDeck(Guid deckId);
    Task RemoveCardOnTop(Guid deckId);
    Task Clear(Guid deckId);
}