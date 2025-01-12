using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnoServices.Interfaces;
using UnoServices.Services;


namespace UnoServices.Configuration;

[ExcludeFromCodeCoverage]
public static class DataServicesConfiguration
{
    public static void AddServicesConfig(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<IPlayerService, PlayerService>();
        serviceCollection.AddScoped<IGameService, GameService>();
        serviceCollection.AddScoped<IRoundService, RoundService>();
        serviceCollection.AddScoped<IPlayerCardService, PlayerCardService>();
        serviceCollection.AddScoped<IGameStateService, GameStateService>();
    }
}