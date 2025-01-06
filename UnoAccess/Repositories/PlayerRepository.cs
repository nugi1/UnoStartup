using Microsoft.EntityFrameworkCore;
using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly DbSet<Player> _players;
    
    public PlayerRepository(UnoContext context)
    {
        _players = context.Set<Player>();
    }

    public async Task<Player?> GetPlayerByIdAsync(Guid playerId)
    {
        return await _players.FindAsync(playerId);
    }

    public async Task<IEnumerable<Player>> GetAllPlayersAsync()
    {
        return await _players.ToListAsync();
    }

    public async Task<Player> CreatePlayerAsync(Player player)
    {
        using (var db = new UnoContext())
        {
            await db.Players.AddAsync(player);

            await db.SaveChangesAsync();
        }
        return player;
    }

    public Player UpdatePlayerAsync(Player player)
    {
        using (var db = new UnoContext())
        {
            db.Players.Update(player);
        }

        return player;
    }

    public async Task DeletePlayerAsync(Guid playerId)
    {
        var player = await _players.FindAsync(playerId);

        if (player != null)
        {
            _players.Remove(player);
        }
    }

    public async Task<Player?> GetPlayerByUsernameAsync(string username)
    {
        var player = await _players.FirstOrDefaultAsync(player => player.Username == username);
        
        return player;
    }

    public async Task<IEnumerable<Player>> GetAllPlayers(string deckId)
    {
        var players = await _players
                                            .Include(p => p.Game)
                                            .Where(p => p.Game.DeckId.ToString() == deckId)
                                            .ToListAsync();

        return players;
    }
}