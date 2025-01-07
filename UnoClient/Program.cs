using Microsoft.AspNetCore.SignalR.Client;
using Shared.DTOs;
using UnoAccess.Entities;

var hubUrl = "http://localhost:5001/hub/uno";

var connection1 = new HubConnectionBuilder()
    .WithUrl(hubUrl)
    .Build();

var connection2 = new HubConnectionBuilder()
    .WithUrl(hubUrl)
    .Build();

Deck? table = null;

connection1.On<Deck>("DeckCreated", (deck) =>
{
    Console.WriteLine($"Deck created with Id {deck.Id}");
    table = deck;
});

connection2.On<List<string>>("PlayerJoined", (players) =>
{
    Console.WriteLine($"Players in the deck: {string.Join(", ", players)}");
});

connection2.On<string>("GameIsStarting", (deckId) =>
{
    Console.WriteLine($"Game is starting for Deck ID: {deckId}");
});

connection2.On<Player>("WaitingForPlayer", (playerWithTurn) =>
{
    Console.WriteLine($"Waiting for player: {playerWithTurn.Id}");
});

try
{
    await connection1.StartAsync();
    await connection2.StartAsync();

    Console.WriteLine("Connected to SignalR server.");

    var deckDto = new CreateDeckDto
    {
        OwnerPlayer = "Player1",
        RequestedQuantity = 2,
        IsPrivate = false,
        Password = null, 
        MaxWaitTime = 30
    };

    await connection1.InvokeAsync("CreateDeck", deckDto);

    await connection2.InvokeAsync("JoinDeck", "Player2", table.Id);

    await connection2.InvokeAsync("GameStart", table.Id);

    await Task.Delay(2000);
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    await connection1.StopAsync();
    await connection2.StopAsync();
}