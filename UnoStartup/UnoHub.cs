using Microsoft.AspNetCore.SignalR;
using Shared.DTOs.ChangeStateData;
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

        public async Task UpdateGameState(PlayerAction action)
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
        }

        public async Task BroadCastChanges(IEnumerable<GameStateChange> changes, Guid gameId)
        {
            foreach (var change in changes)
            {
                switch (change.Type)
                {
                    case "GameCreated":
                        var gameCreatedDto = change.Data as GameCreatedDto;
                        await Groups.AddToGroupAsync(Context.ConnectionId, gameCreatedDto.GameId.ToString());
                        break;
                    case "GamePlayersChanged":
                        var players = change.Data as List<PlayerState>;
                        var currentGameId = players[0].GameId.ToString();

                        foreach (var player in players)
                        {
                            await Groups.AddToGroupAsync(player.ConnectionId, currentGameId);
                        }

                        await Clients.Group(currentGameId).SendAsync("GamePlayersChanged", players);
                        break;
                    case "RoundStarted":
                        break;
                    case "TopCardChanged":
                        var topCard = change.Data as GameTopCardDto;
                        await Clients.Group(topCard.GameId.ToString()).SendAsync("NewTopCard", topCard.Card);
                        break;
                    case "PlayerCardsChanged":
                        var cards = change.Data as GamePlayerCards;
                        await Clients.Client(cards.ConnectionId).SendAsync("NewPlayerCards", cards);
                        break;
                    case "PlayerScoreChanged":
                        var playerScore = change.Data as GameScoreDto;
                        await Clients.Client(playerScore.ConnectionId).SendAsync("NewPlayerScore", playerScore.Score);
                        break;
                    case "ChangedTurn":
                        var playerTurn = change.Data as GameTurnDto;
                        await Clients.Group(playerTurn.GameId.ToString()).SendAsync("NewTurn", playerTurn.Username);
                        await Clients.Client(playerTurn.ConnectionId).SendAsync("YourTurn", playerTurn.Username);
                        break;
                    case "NewRound":
                        var newRound = change.Data as GameRoundDto;
                        await Clients.Group(newRound.GameId.ToString()).SendAsync("NewRoundStarting", newRound.Round);
                        break;
                    case "WonGame":
                        var winner = change.Data as GameWinnerDto;
                        await Clients.Group(winner.GameId.ToString()).SendAsync("WinnerOfTheGame", change.Data);
                        await Clients.Client(winner.ConnectionId).SendAsync("YouWon", change.Data);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}