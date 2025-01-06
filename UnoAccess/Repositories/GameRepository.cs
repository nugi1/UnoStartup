using Microsoft.EntityFrameworkCore;
using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class GameRepository : IGameRepository
{
    private DbSet<Game> Games { get; set; }
    
    public GameRepository(UnoContext context)
    {
        Games = context.Set<Game>();
    }
    
    public async Task<Game?> GetGameByIdAsync(Guid gameId)
    {
        return await Games.FindAsync(gameId);
    }

    public async Task<IEnumerable<Game>> GetAllGamesAsync()
    {
        return await Games.ToListAsync();
    }

    public async Task<Game> CreateGameAsync(Game game)
    {
        using (var db = new UnoContext())
        {
            await db.Games.AddAsync(game);

            await db.SaveChangesAsync();
        }

        return game;
    }

    public Game UpdateGameAsync(Game game)
    {
        Games.Update(game);

        return game;
    }

    public async Task DeleteGameAsync(Guid gameId)
    {
        var game = await Games.FirstOrDefaultAsync(g => g.Id == gameId);

        if (game != null)
        {
            Games.Remove(game);
        }
    }
}