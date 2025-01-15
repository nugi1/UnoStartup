using UnoAccess.Entities;

namespace UnoServices.Interfaces;

public interface IGameService
{
    Task<int> GetPlayersQuantity(Guid? gameId);

    Task UpdateRoundId(Guid gameId, Guid roundId);
    Task<Round> GetRound(Guid? gameId);
    Task<bool> GetDirection(Guid gameId);
    Task<bool> ChangeDirection(Guid gameId);
    Task NewRoundStartedAsync(Round round);
}