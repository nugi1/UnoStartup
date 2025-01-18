using UnoAccess.Entities;

namespace UnoServices.Interfaces;

public interface IPlayerService
{
    Task ChangePlayerTurnAsync(Player player);

    Task<IEnumerable<Player>> GetPlayersBy(Guid gameId);

    Task<int> GetHighestPosition(Guid gameId);

    Task AddPlayerAsync(Player player);

    Task<Player?> SetTurn(bool direction, Guid gameId, int currentPosition, int steps, int quantity);
    Task<Player> GetTurn(Guid gameId);
    Task<Player> GetNextPlayer(bool direction, int position, Guid? gameId, int quantity);
    Task ReversePositions(Guid? gameId);
    Task UpdatePlayerScoreAsync(Player player, int score);
    Task SaidUnoAsync(Player player);
}