using UnoAccess.Entities;

namespace UnoAccess.Interfaces;

public interface IPlayerRepository
{
    Task<Player?> GetPlayerByIdAsync(Guid playerId);
    Task<IEnumerable<Player>> GetAllPlayersAsync();
    Task<Player> CreatePlayerAsync(Player player);
    Player UpdatePlayerAsync(Player player);
    Task DeletePlayerAsync(Guid playerId);
    Task<Player?> GetPlayerByUsernameAsync(string username);
    Task<IEnumerable<Player>> GetAllPlayers(string deckId);
}