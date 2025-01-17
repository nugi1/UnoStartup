using Microsoft.EntityFrameworkCore;
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

    public async Task<int> GetPlayersQuantity(Guid? gameId)
    {
        var quantity = (await _unitOfWork.Players.ReadAsync(g => g.GameId == gameId, noTracking:true)).Count();

        return quantity;
    }

    public async Task<int> GetRequestedQuantity(Guid? gameId)
    {
        var result = (await _unitOfWork.Games.ReadByIdAsync(gameId)).PlayerQuantity;

        return result;
    }


    public async Task UpdateRoundId(Guid gameId, Guid roundId)
    {
        var game = await _unitOfWork.Games.ReadByIdAsync(gameId);
        game.RoundId = roundId;
        
        _unitOfWork.Games.Update(game);
        await _unitOfWork.SaveAsync();
    }

    public async Task<Round> GetRound(Guid? gameId)
    {
        var game = await _unitOfWork.Games.ReadOneAsync(
            predicate: g => g.Id == gameId,
            include: g => g.Include(e => e.Round).ThenInclude(r => r.TopCard),
            noTracking:true);

        return game.Round;
    }

    public async Task<bool> GetDirection(Guid gameId)
    {
        var game = await _unitOfWork.Games.ReadByIdAsync(gameId);

        return game.Clockwise;
    }

    public async Task<bool> ChangeDirection(Guid gameId)
    {
        var game = await _unitOfWork.Games.ReadByIdAsync(gameId);

        if (game.Clockwise)
        {
            game.Clockwise = false;
        }
        else
        {
            game.Clockwise = true;
        }

        _unitOfWork.Games.Update(game);
        await _unitOfWork.SaveAsync();
        
        return game.Clockwise;
    }

    public async Task NewRoundStartedAsync(Round round)
    {
        round.Number += 1;
        await _unitOfWork.SaveAsync();
    }

    public async Task WinGameAsync(Player player, Guid? gameId)
    {
        var game = await _unitOfWork.Games.ReadByIdAsync(gameId ?? throw new InvalidOperationException());

        game.WinnerName = player.Username;
        player.IsWinner = true;

        _unitOfWork.Games.Update(game);
        _unitOfWork.Players.Update(player);
    }
}