using Shared.DTOs;

namespace UnoServices.Interfaces;

public interface IDiscardPileService
{
    Task AddCardOnTop(Guid deckId, CardDto cardDto);
}