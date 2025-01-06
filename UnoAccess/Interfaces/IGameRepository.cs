using UnoAccess.Entities;

namespace UnoAccess.Interfaces;

public interface IGameRepository
{
    Task<Game?> GetGameByIdAsync(Guid gameId);
    Task<IEnumerable<Game>> GetAllGamesAsync();
    Task<Game> CreateGameAsync(Game game);
    Game UpdateGameAsync(Game game);
    Task DeleteGameAsync(Guid gameId);
}