using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoAccess.Repositories;

namespace UnoAccess;

public static class Configure
{
    public static void ConfigureUnoAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UnoContext>();

        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<IDeckRepository, DeckRepository>();
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<ICardDeckRepository, CardDeckRepository>();
        services.AddScoped<ICardPlayerRepository, CardPlayerRepository>();
        services.AddScoped<IDiscardPileDeckRepository, DiscardPileDeckRepository>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}