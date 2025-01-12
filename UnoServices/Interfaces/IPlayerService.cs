using UnoAccess.Entities;

namespace UnoServices.Interfaces;

public interface IPlayerService
{
    Task ChangePlayerTurnAsync(Player player, int playerQuantity);

    Task<IEnumerable<Player>> GetPlayersBy(Guid gameId);

    Task<int> GetHighestPosition(Guid gameId);

    Task AddPlayerAsync(Player player);

    Task SetTurn(Guid gameId, int position);
    Task<Player> GetTurn(Guid? gameId);
}