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

    public async Task ChangePlayerTurnAsync(Player player)
    {
        player.IsTurn = false;

        _unitOfWork.Players.Update(player);

        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<Player>> GetPlayersBy(Guid gameId)
    {
        var players = await _unitOfWork.Players.ReadAsync(
            predicate: p => p.GameId == gameId,
            noTracking: true);

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
            predicate: p => p.GameId == gameId && p.IsTurn,
            noTracking:true);

        return player;
    }

    public async Task<Player> GetNextPlayer(bool direction, int position, Guid? gameId, int quantity)
    {
        if (direction)
        {
            position += 1;
        }
        else
        {
            position -= 1;
        }
        
        var calculatedPosition = (position % quantity + quantity) % quantity;

        var player = await _unitOfWork.Players.ReadOneAsync(
            predicate: p => p.GameId == gameId && p.Position == calculatedPosition,
            noTracking:true);

        return player;
    }

    public async Task ReversePositions(Guid? gameId)
    {
        var players = await _unitOfWork.Players.ReadAsync(
            predicate: p => p.GameId == gameId);

        foreach (var player in players)
        {
            player.Position = players.Count - player.Position - 1;

            _unitOfWork.Players.Update(player);
        }

        await _unitOfWork.SaveAsync();
    }

    public async Task UpdatePlayerScoreAsync(Player player, int score)
    {
        player.Score = score;

        _unitOfWork.Players.Update(player);
        await _unitOfWork.SaveAsync();
    }

    public async Task SaidUnoAsync(Player player)
    {
        player.UnoSaid = true;

        _unitOfWork.Players.Update(player);
        await _unitOfWork.SaveAsync();
    }

    public async Task<Player?> SetTurn(bool direction, Guid gameId, int currentPosition, int steps, int quantity)
    {
        if (!direction)
        {
            steps *= -1;
        }
        
        var calculatedPosition = ((currentPosition + steps) % quantity + quantity) % quantity;
        
        var player = await _unitOfWork.Players.ReadOneAsync(
            predicate: p => p.Position == calculatedPosition % quantity && p.GameId == gameId && p.IsTurn == false,
            noTracking: true);

        if (player != null)
        {
            player.IsTurn = true;
            _unitOfWork.Players.Update(player);
        }

        await _unitOfWork.SaveAsync();

        return player;
    }
}