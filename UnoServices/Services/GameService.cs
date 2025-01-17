using Microsoft.EntityFrameworkCore;
using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoServices.Interfaces;

namespace UnoServices.Services;

public class GameService(IUnitOfWork unitOfWork) : IGameService
{
    public async Task<int> GetPlayersQuantity(Guid gameId)
    {
        var quantity = (await unitOfWork.Players.ReadAsync(g => g.GameId == gameId, noTracking:true)).Count;

        return quantity;
    }

    public async Task<int> GetRequestedQuantity(Guid gameId)
    {
        var result = await unitOfWork.Games.ReadByIdAsync(gameId)
                     ?? throw new InvalidDataException("Game not found!");

        return result.PlayerQuantity;
    }


    public async Task UpdateRoundId(Guid gameId, Guid roundId)
    {
        var game = await unitOfWork.Games.ReadByIdAsync(gameId) 
                   ?? throw new InvalidDataException("Game not found!");

        game.RoundId = roundId;
        unitOfWork.Games.Update(game);
        
        await unitOfWork.SaveAsync();
    }

    public async Task<Round> GetRound(Guid gameId)
    {
        var game = await unitOfWork.Games.ReadOneAsync(
            predicate: g => g.Id == gameId,
            include: g => g.Include(e => e.Round).ThenInclude(r => r!.TopCard),
            noTracking:true) ?? throw new InvalidDataException("Game not found!");

        return game.Round ?? throw new InvalidDataException("Game is not started yet!");
    }

    public async Task<bool> GetDirection(Guid gameId)
    {
        var game = await unitOfWork.Games.ReadByIdAsync(gameId)
            ?? throw new InvalidDataException("Game not found!");

        return game.Clockwise;
    }

    public async Task<bool> ChangeDirection(Guid gameId)
    {
        var game = await unitOfWork.Games.ReadByIdAsync(gameId)
            ?? throw new InvalidDataException("Game not found!");

        game.Clockwise = !game.Clockwise;

        unitOfWork.Games.Update(game);
        await unitOfWork.SaveAsync();
        
        return game.Clockwise;
    }

    public async Task NewRoundStartedAsync(Round round)
    {
        round.Number += 1;
        await unitOfWork.SaveAsync();
    }

    public async Task WinGameAsync(Player player, Guid gameId)
    {
        var game = await unitOfWork.Games.ReadByIdAsync(gameId) 
                   ?? throw new InvalidDataException("Game not found!");

        game.WinnerName = player.Username;
        player.IsWinner = true;

        unitOfWork.Games.Update(game);
        unitOfWork.Players.Update(player);
    }
}