using Shared.States;
using UnoAccess.Entities;
using UnoServices.Delegates;

namespace UnoServices.Interfaces;

public interface IGameStateService
{
    Task<GameState> GetGameState(Guid gameId);
    Task<GameState> UpdateGameState(PlayerAction action);
    IEnumerable<GameStateChange> GetGameStateChanges(GameState? oldState, GameState newState);
}
