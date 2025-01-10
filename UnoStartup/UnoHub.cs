using Microsoft.AspNetCore.SignalR;
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
            var state = await _gameStateService.UpdateGameState(action);

            return state;
        }
    }
}
