using Shared.DTOs;
using UnoAccess.Entities;

namespace UnoServices.Interfaces;

public interface IGameService
{
    Task<Game> CreateGame(Guid deckId);

    Task<Game> GameOver(Guid gameId, Guid winnerId);

    Task<Game?> GetGameBy(Guid gameId);
}