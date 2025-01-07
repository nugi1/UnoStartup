using System.Collections.Concurrent;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.SignalR;
using Shared.DTOs;
using UnoAccess.Entities;
using UnoServices.Interfaces;

namespace UnoStartup.Hub;

public class UnoHub : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly IDeckService _deckService;
    private readonly IGameService _gameService;
    private readonly IPlayerService _playerService;
    private readonly ICardDeckService _cardDeckService;
    private readonly ICardPlayerService _cardPlayerService;
    private readonly IDiscardPileService _discardPileService;
    private readonly ICardService _cardService;
    private static readonly ConcurrentDictionary<string, List<Player>> groupConnections = new ConcurrentDictionary<string, List<Player>>();

    public UnoHub(
        IDeckService deckService,
        IGameService gameService,
        IPlayerService playerService,
        ICardDeckService cardDeckService,
        ICardPlayerService cardPlayerService,
        IDiscardPileService discardPileService,
        ICardService cardService)
    {
        _deckService = deckService;
        _gameService = gameService;
        _playerService = playerService;
        _cardDeckService = cardDeckService;
        _cardPlayerService = cardPlayerService;
        _discardPileService = discardPileService;
        _cardService = cardService;
    }

    // TODO: Organize database entities and integrate with Identity.
    // TODO: Build the Console App, that acts like a client
    // TODO: Error Handling

    public async Task<Deck> CreateDeck(CreateDeckDto deckDto)
    {
        var deck = await _deckService.CreateDeck(deckDto);

        groupConnections[deck.Id.ToString()] = new List<Player>()
        {
            new Player()
            {
                ConnectionId = Context.ConnectionId,
                Username = deckDto.OwnerPlayer,
                Score = 0,
                IsActive = true,
                Position = 0,
                DeckId = deck.Id
            }
        };

        await Groups.AddToGroupAsync(Context.ConnectionId, deck.Id.ToString());

        await Clients.Group(deck.Id.ToString()).SendAsync("DeckCreated", deck);

        return deck;
    }

    public async Task JoinDeck(string playerName, Guid deckId)
    {
        var deck = await _deckService.ReadDeck(deckId);

        if (deck == null)
        {
            await Clients.Caller.SendAsync("Error", "Deck not found or invalid.");
            return;
        }

        if (groupConnections[deck.Id.ToString()].All(p => p.ConnectionId != Context.ConnectionId) && groupConnections[deck.Id.ToString()].Count < deck.PlayersCount)
        {
            groupConnections[deck.Id.ToString()].Add(
                new Player()
                {
                    ConnectionId = Context.ConnectionId,
                    Username = playerName,
                    Score = 0,
                    IsActive = true,
                    Position = groupConnections[deck.Id.ToString()].Count,
                    DeckId = deckId
                }
            );
            await Groups.AddToGroupAsync(Context.ConnectionId, deck.Id.ToString());
            await Clients.Group(deck.Id.ToString()).SendAsync("PlayerJoined", groupConnections[deckId.ToString()].Select(p => p.Username).ToList());
            System.Console.WriteLine(groupConnections[deckId.ToString()]);
        }

        if (groupConnections[deck.Id.ToString()].Count == deck.PlayersCount)
        {
            await Clients.Caller.SendAsync("GameIsStarting", deck.Id.ToString());
        }
    }

    public async Task GameStart(Guid deckId)
    {
        var game = await _gameService.CreateGame(deckId);
        await _playerService.AddPlayers(game.Id, groupConnections[deckId.ToString()]);
        await Clients.Group(deckId.ToString()).SendAsync("GameStarted", deckId.ToString());
        await _cardDeckService.FillTheDeck(deckId);
        await _cardPlayerService.FillThePlayers(deckId);
        await Clients.Group(deckId.ToString()).SendAsync("FillingCards", deckId);
        await _deckService.SetTurn(0, deckId);
        var playerWithTurn = await _deckService.GetPlayerWithTurn(deckId);
        await Clients.Group(deckId.ToString()).SendAsync("WaitingForPlayer", playerWithTurn);
    }

    public async Task GameRefresh(Guid deckId)
    {
        await _cardDeckService.FillTheDeck(deckId);
        await _cardPlayerService.FillThePlayers(deckId);
        await Clients.Group(deckId.ToString()).SendAsync("FillingCards", deckId);
        var playerWithTurn = await _deckService.GetPlayerWithTurn(deckId);
        await Clients.Group(deckId.ToString()).SendAsync("WaitingForPlayer", playerWithTurn);
    }

    public async Task PlayerMakesTurn(Guid playerId, Guid deckId, CardDto cardDto)
    {
        var playerWithTurn = await _deckService.GetPlayerWithTurn(deckId);

        if (playerWithTurn.Id != playerId)
        {
            await Clients.Caller.SendAsync("CantMove", $@"Invalid request to take turn {playerId}!");
            return;
        }

        var deck = await _deckService.ReadDeck(deckId);

        if (deck == null)
        {
            await Clients.Caller.SendAsync("DeckDoesNotExist", playerId, deckId, cardDto);
            return;
        }

        var topCard = deck.TopCard;
        if (topCard != null && !IsValidCardPlay(topCard, cardDto))
        {
            await Clients.Caller.SendAsync("CantMove", "The played card is invalid!", cardDto);
            return;
        }

        await _discardPileService.AddCardOnTop(deckId, cardDto);
        await Clients.Group(deckId.ToString()).SendAsync("MadeTurn", new { Card = cardDto, Name = playerWithTurn.ToString() });

        /* Just for testing
        var cardId = cards.First().CardId;
        var card = await _cardService.GetCard(cardId);
        cardDto.Value = card.Value;
        cardDto.Color = card.Color;
        */

        await _cardPlayerService.RemovePlayerCard(cardDto, deckId, playerWithTurn.Id);
        await Clients.Caller.SendAsync("RemovedPlayerCard", cardDto);
        switch (cardDto.Value)
        {
            case CardValue.Reverse:
                await _deckService.TogglePlayDirection(deckId); // Toggle the play direction
                break;
            case CardValue.Skip:
                playerWithTurn = await _deckService.SetTurn(playerWithTurn.Position + 1, deckId);// Skip the next player
                break;
            case CardValue.DrawTwo:
                playerWithTurn = await _deckService.SetTurn(playerWithTurn.Position + 1, deckId);
                await _cardPlayerService.GiveCardsToPlayer(deckId, playerWithTurn.Id, 2); // Give two cards to the next player
                break;
            case CardValue.WildDrawFour:
                playerWithTurn = await _deckService.SetTurn(playerWithTurn.Position + 1, deckId);
                await _cardPlayerService.GiveCardsToPlayer(deckId, playerWithTurn.Id, 4);
                break;
        }

        var cards = (await _cardPlayerService.GetPlayerCards(playerWithTurn.Id)).ToList();

        // TODO: If uno is said by player and cards.Count == 0, then refresh the round and calculate points to be added.
        if (playerWithTurn.UnoSaid && cards.Count == 0)
        {
            var score = await _playerService.CountScore(playerId, deckId);

            await _deckService.SetScoreAsync(playerId, deckId, score);
            await _deckService.RefreshDeck(deckId);

            await Clients.Group(deck.Id.ToString()).SendAsync("NewRound", deckId);

            var newScore = await _playerService.CountScore(playerId, deckId);

            if (newScore >= 500)
            {
                await Clients.Group(deck.Id.ToString()).SendAsync("GameOver", playerId);
                var game = await _gameService.GameOver(playerWithTurn.GameId, playerId);
                await _cardPlayerService.Clear(game.Players.Select(p => p.Id));
                await _cardDeckService.Clear(deckId);
            }
            return;
        }

        var player = await _deckService.SetTurn(playerWithTurn.Position + 1, deckId);
        await Clients.Group(deckId.ToString()).SendAsync("NowTurn", player.Username);
    }

    public async Task CanSayUno(Guid playerId)
    {
        var cards = (await _cardPlayerService.GetPlayerCards(playerId)).ToList();

        if (cards.Count == 1)
        {
            await _playerService.SayUnoTill(playerId);
            await Clients.Caller.SendAsync("CanCallUno", playerId);
        }
    }

    public async Task DidNotSayUno(Guid playerId, Guid deckId)
    {
        var player = await _playerService.GetPlayerById(playerId);

        if (player.SayUnoUntil > DateTime.Now && player.UnoSaid == false)
        {
            await _cardPlayerService.GiveCardsToPlayer(deckId, playerId, 2);
            await Clients.Caller.SendAsync("Penalty", playerId);
        }
    }

    public async Task SaysUno(Guid playerId)
    {
        var cards = (await _cardPlayerService.GetPlayerCards(playerId)).ToList();

        if (cards.Count == 1)
        {
            var player = await _playerService.GetPlayerById(playerId);
            if (player.SayUnoUntil <= DateTime.Now)
            {
                await _playerService.SayUno(playerId);
                await Clients.Caller.SendAsync("SaidUno", playerId);
            }
        }
        else
        {
            await Clients.Caller.SendAsync("CantSayUno", playerId);
            return;
        }
    }

    public async Task DisconnectFromDeck(string playerName, Guid deckId)
    {
        var deck = await _deckService.ReadDeck(deckId);

        if (deck == null)
        {
            await Clients.Caller.SendAsync("Error", "Deck not found or invalid.");
            return;
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, deck.Id.ToString());

        await Clients.Group(deck.Id.ToString()).SendAsync("PlayerDisconnected", playerName);

        var player = groupConnections[deck.Id.ToString()].FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
        groupConnections[deck.Id.ToString()].Remove(player);
    }

    private bool IsValidCardPlay(Card topCard, CardDto playedCard)
    {
        if (playedCard.Value == CardValue.Wild || playedCard.Value == CardValue.WildDrawFour)
        {
            return true;
        }

        if (topCard.Color == playedCard.Color)
        {
            return true;
        }

        if (topCard.Value == playedCard.Value)
        {
            return true;
        }

        return false;
    }
}