using UnoAccess.Entities;

namespace UnoServices.Interfaces;

public interface IPlayerService
{
    Task<List<Player>> AddPlayers(Guid gameId, List<Player> players);
    Task<Player> SayUno(Guid playerId);

    Task<Player> SayUnoTill(Guid playerId);

    Task<Player> GetPlayerById(Guid playerId);

    Task<int> CountScore(Guid playerId, Guid deckId);
}