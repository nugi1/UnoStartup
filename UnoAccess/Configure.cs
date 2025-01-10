using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnoAccess.Interfaces;
using UnoAccess.Repositories;
using UnoAccess.Context;

namespace UnoAccess.Configuration;

[ExcludeFromCodeCoverage]
public static class DataAccessConfiguration
{
    public static void AddAccessConfig(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<UnoContext>((sp, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("GameDatabase"));
        });

        serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        serviceCollection.AddScoped<IGameRepository, GameRepository>();
        serviceCollection.AddScoped<IRoundRepository, RoundRepository>();
        serviceCollection.AddScoped<IRoundCardRepository, RoundCardRepository>();
        serviceCollection.AddScoped<ICardRepository, CardRepository>();
        serviceCollection.AddScoped<IPlayerRepository, PlayerRepository>();

        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}