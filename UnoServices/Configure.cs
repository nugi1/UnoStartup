using Microsoft.Extensions.DependencyInjection;
using UnoServices.Interfaces;
using UnoServices.Services;

namespace UnoServices;

public static class Configure
{
    public static void ConfigureUnoServices(this IServiceCollection services)
    {
        services.AddScoped<IDeckService, DeckService>();
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<ICardDeckService, CardDeckService>();
        services.AddScoped<ICardPlayerService, CardPlayerService>();
        services.AddScoped<IDiscardPileService, DiscardPileService>();
    }
}