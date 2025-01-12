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
        var quantity = (await _unitOfWork.Players.ReadAsync(g => g.GameId == gameId)).Count();

        return quantity;
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
            include: g => g.Include(e => e.Round).ThenInclude(r => r.TopCard));

        return game.Round;
    }
}