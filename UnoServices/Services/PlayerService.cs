using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoServices.Interfaces;

namespace UnoServices.Services;

public class PlayerService(IUnitOfWork unitOfWork) : IPlayerService
{
    public async Task ChangePlayerTurnAsync(Player player)
    {
        var trackedEntity = unitOfWork.Players.GetTrackedEntity(player.Id);

        if (trackedEntity != null)
        {
            unitOfWork.Players.DetachEntity(trackedEntity);
        }
        player.IsTurn = false;
        unitOfWork.Players.Update(player);

        await unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<Player>> GetPlayersBy(Guid gameId)
    {
        var players = await unitOfWork.Players.ReadAsync(
            predicate: p => p.GameId == gameId,
            noTracking: true);

        return players;
    }

    public async Task<int> GetHighestPosition(Guid gameId)
    {
        var position = (await unitOfWork.Players.ReadAsync(
            predicate: p => p.GameId == gameId,
            noTracking:true)).OrderByDescending(p => p.Position).First().Position;

        return position;
    }

    public async Task AddPlayerAsync(Player player)
    {
        await unitOfWork.Players.CreateAsync(player);

        await unitOfWork.SaveAsync();
    }

    public async Task<Player> GetTurn(Guid gameId)
    {
        var player = await unitOfWork.Players.ReadOneAsync(
            predicate: p => p.GameId == gameId && p.IsTurn,
            noTracking:true) ?? throw new InvalidOperationException("Something wrong!");

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

        var player = await unitOfWork.Players.ReadOneAsync(
            predicate: p => p.GameId == gameId && p.Position == calculatedPosition,
            noTracking:true);

        return player;
    }

    public async Task ReversePositions(Guid? gameId)
    {
        var players = await unitOfWork.Players.ReadAsync(
            predicate: p => p.GameId == gameId,
            noTracking:true);

        foreach (var player in players)
        {
            player.Position = players.Count - player.Position - 1;

            unitOfWork.Players.Update(player);
        }

        await unitOfWork.SaveAsync();
    }

    public async Task UpdatePlayerScoreAsync(Player player, int score)
    {
        player.Score += score;

        unitOfWork.Players.Update(player);
        await unitOfWork.SaveAsync();
    }

    public async Task SaidUnoAsync(Player player)
    {
        player.UnoSaid = true;

        unitOfWork.Players.Update(player);
        await unitOfWork.SaveAsync();
    }

    public async Task SayUnoUntil(Player player)
    {
        player.SayUnoUntil = DateTime.Now.AddSeconds(5).ToUniversalTime();
        
        unitOfWork.Players.Update(player);
        await unitOfWork.SaveAsync();
    }

    public async Task RemoveUnoSaidAsync(Guid gameId)
    {
        var players = await unitOfWork.Players.ReadAsync(
            predicate: p => p.GameId == gameId,
            noTracking: true);

        foreach (var player in players)
        {
            var trackedEntity = unitOfWork.Players.GetTrackedEntity(player.Id);
            if (trackedEntity != null)
            {
                unitOfWork.Players.DetachEntity(trackedEntity);
            }
            
            unitOfWork.Players.Update(player);
        }

        await unitOfWork.SaveAsync();
    }

    public async Task<Player?> SetTurn(bool direction, Guid gameId, int currentPosition, int steps, int quantity)
    {
        if (!direction)
        {
            steps *= -1;
        }
        
        var calculatedPosition = ((currentPosition + steps) % quantity + quantity) % quantity;
        
        var player = await unitOfWork.Players.ReadOneAsync(
            predicate: p => p.Position == calculatedPosition % quantity && p.GameId == gameId && p.IsTurn == false,
            noTracking: true);
        
        if (player != null)
        {
            var trackedEntity = unitOfWork.Players.GetTrackedEntity(player.Id);
            if (trackedEntity != null)
            {
                unitOfWork.Players.DetachEntity(trackedEntity);
            }
            player.IsTurn = true;
            unitOfWork.Players.Update(player);
        }

        await unitOfWork.SaveAsync();

        return player;
    }
}