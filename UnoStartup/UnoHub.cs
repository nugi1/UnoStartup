using Microsoft.AspNetCore.SignalR;
using Shared.States;
using UnoAccess.Entities;
using UnoServices.Delegates;
using UnoServices.Interfaces;

namespace UnoStartup
{
    public class UnoHub : Hub
    {
        private readonly IGameStateService _gameStateService;

        public UnoHub(IGameStateService gameStateService) 
        {
            _gameStateService = gameStateService;
        }

        public async Task<GameState> UpdateGameState(PlayerAction action)
        {
            GameState? oldState = null;
            if (action.GameId != null)
            {
                oldState = await _gameStateService.GetGameState(action.GameId ?? throw new NotImplementedException());
            }

            action.ConnectionId = Context.ConnectionId;
            var newState = await _gameStateService.UpdateGameState(action);

            var changes = _gameStateService.GetGameStateChanges(oldState, newState);

            await BroadCastChanges(changes, newState.GameId ?? throw new NotImplementedException());

            return newState;
        }

        public async Task BroadCastChanges(IEnumerable<GameStateChange> changes, Guid gameId)
        {
            foreach (var change in changes)
            {
                switch (change.Type)
                {
                    case "WaitingForPlayers":
                        break;
                    case "GamePlayersChanged":
                        break;
                    case "RoundStarted":
                        break;
                    case "TopCardChanged":
                        break;
                    case "PlayerCardsChanged":
                        break;
                    case "PlayerScoreChanged":
                        break;
                    case "ChangedTurn":
                        break;
                    case "NewRound":
                        break;
                    case "WonGame":
                        break;
                    default:
                        break;
                }
            }
    }
}
