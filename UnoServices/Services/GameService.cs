using Shared.DTOs;
using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoServices.Interfaces;

namespace UnoServices.Services;

public class GameService : IGameService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GameService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Game> CreateGame(Guid deckId)
    {
        var game = new Game()
        {
            DeckId = deckId,
        };

        await _unitOfWork.GameRepository.CreateGameAsync(game);

        return game;
    }

    public async Task<Game> GameOver(Guid gameId, Guid winnerId)
    {
        var game = await _unitOfWork.GameRepository.GetGameByIdAsync(gameId);

        game.WinnerId = winnerId;

        _unitOfWork.GameRepository.UpdateGameAsync(game);

        return game;
    }
}