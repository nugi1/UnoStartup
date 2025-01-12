using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoServices.Interfaces;

namespace UnoServices.Services;

public class PlayerService : IPlayerService
{
    private readonly IUnitOfWork _unitOfWork;

    public PlayerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task ChangePlayerTurnAsync(Player player, int playerQuantity)
    {
        player.IsTurn = false;

        var nextPlayer = await _unitOfWork.Players.ReadOneAsync(
            predicate: p => p.Position == (player.Position + 1) % playerQuantity);

        _unitOfWork.Players.Update(player);

        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<Player>> GetPlayersBy(Guid gameId)
    {
        var players = await _unitOfWork.Players.ReadAsync(
            predicate: p => p.GameId == gameId);

        return players;
    }

    public async Task<int> GetHighestPosition(Guid gameId)
    {
        var position = (await _unitOfWork.Players.ReadAsync(
            predicate: p => p.GameId == gameId)).OrderByDescending(p => p.Position).First().Position;

        return position;
    }

    public async Task AddPlayerAsync(Player player)
    {
        await _unitOfWork.Players.CreateAsync(player);

        await _unitOfWork.SaveAsync();
    }

    public async Task<Player> GetTurn(Guid? gameId)
    {
        var player = await _unitOfWork.Players.ReadOneAsync(
            predicate: p => p.GameId == gameId && p.IsTurn);

        return player;
    }

    public async Task SetTurn(Guid gameId, int position)
    {
        var player = await _unitOfWork.Players.ReadOneAsync(
            predicate: p => p.Position == position && p.GameId == gameId);

        player.IsTurn = true;
        
        _unitOfWork.Players.Update(player);
        await _unitOfWork.SaveAsync();
    }
}