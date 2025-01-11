using Microsoft.AspNetCore.SignalR.Client;
using Shared.DTOs.Actions;
using UnoAccess.Entities;
using UnoServices.Delegates;

var hubUrl = "http://localhost:5001/uno/hub";

var connection = new HubConnectionBuilder().WithUrl(hubUrl).Build();

var connection_ = new HubConnectionBuilder().WithUrl(hubUrl).Build();

try
{
    await connection.StartAsync();
    var playerAction = new PlayerAction()
    {
        ActionType = "CreateGame",
        ActionData = new CreateGameDto(){
            PlayerName = "Nugi",
            RequestedQuantity = 2,
            Password = null
        }
    };

    var state = await connection.InvokeAsync<GameState>("UpdateGameState", playerAction);

    Console.WriteLine($"{state.GameId} created");


    playerAction = new PlayerAction
    {
        ActionType = "JoinGame",
        ActionData = new JoinGameDto()
        {
            Username = "Luka",
            GameId = state.GameId ?? throw new NotImplementedException(),
        }
    };
    await connection_.StartAsync();

    state = await connection_.InvokeAsync<GameState>("UpdateGameState", playerAction);

    Console.WriteLine($"Round started for Game with Id: {state.GameId}. " +
                      $"\n Round Id: {state.RoundId}" +
                      $"\n Players: {state.Game.Players.Select(p => p.Username)}");
}
catch
{

}