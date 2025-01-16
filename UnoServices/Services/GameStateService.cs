using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Actions;
using Shared.DTOs.ChangeStateData;
using Shared.Enums;
using Shared.States;
using System;
using System.Numerics;
using System.Text.Json;
using Shared.DTOs;
using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoServices.Delegates;
using UnoServices.Interfaces;

namespace UnoServices.Services;

public class GameStateService : IGameStateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoundService _roundService;
    private readonly IPlayerCardService _playerCardService;
    private readonly IGameService _gameService;

    private readonly List<DelegateAction> _actionDelegates;
    private readonly IPlayerService _playerService;

    public GameStateService(IUnitOfWork unitOfWork, IRoundService roundService, IPlayerCardService playerCardService, IPlayerService playerService, IGameService gameService)
    {
        _unitOfWork = unitOfWork;
        _roundService = roundService;
        _playerCardService = playerCardService;
        _playerService = playerService;
        _gameService = gameService;

        _actionDelegates = new List<DelegateAction>()
        {
            new DelegateAction()
            {
                ActionName = "CreateGame",
                Action = CreateGame
            },
            new DelegateAction()
            {
                ActionName = "JoinGame",
                Action = JoinGame
            },
            new DelegateAction()
            {
                ActionName = "PlayCard",
                Action = PlayCard
            },
            new DelegateAction()
            {
                ActionName = "DrawCard",
                Action = DrawCard
            },
            new DelegateAction()
            {
                ActionName = "PlayWildDrawTwo",
                Action = PlayedDrawTwo
            },
            new DelegateAction()
            {
                ActionName = "PlayWildDrawFour",
                Action = PlayedDrawFour
            },
            new DelegateAction()
            {
                ActionName = "ChallengeWildDrawFour",
                Action = ChallengeWildDrawFour
            },
            new DelegateAction
            {
                ActionName = "UnoDeclaration",
                Action = UnoDeclaration
            },
            new DelegateAction
            {
                ActionName = "WinGame",
                Action = WinGame
            }
        };
    }

    public async Task<GameState> UpdateGameState(PlayerAction action)
    {
        var act = _actionDelegates.Where(ad => ad.ActionName == action.ActionType).FirstOrDefault();

        if (act != null)
        {
            var state = await act.Action(action);

            return state;
        }

        throw new NotImplementedException();
    }

    public async Task<GameState> GetGameState(Guid gameId)
    {
        var game = await _unitOfWork.Games.ReadOneAsync(
            predicate: e => e.Id == gameId,
            include: e => e.Include(g => g.Round)
                                .ThenInclude(r => r.TopCard)
                            .Include(g => g.Round)
                                .ThenInclude(r => r.CardsInPlay)
                            .Include(g => g.Players)
                            .Include(g => g.Round)
                            .Include(g => g.Players)

        ) ?? throw new InvalidOperationException();

        IList<PlayerHand> playerHands = new List<PlayerHand>();
        foreach (var player in game.Players)
        {
            var cards = await _unitOfWork.PlayerCards.GetPlayerCards(player.Id);

            var playerCards = new PlayerHand()
            {
                PlayerId = player.Id,
                Cards = cards.Select(c => new CardDto()
                {
                    Color = c.Color,
                    Value = c.Value
                }).ToList()
            };
            playerHands.Add(playerCards);
        }

        var result = new GameState()
        {
            GameId = gameId,
            Game = game,
            PlayerHands = playerHands,
            RoundId = game.RoundId,
            GameStarted = game.Round != null
        };

        return result;
    }

    private async Task<Game> GetGame(Guid gameId)
    {
        var game = await _unitOfWork.Games.ReadOneAsync(
                        predicate: e => e.Id == gameId,
                        include: e => e.Include(g => g.Round)
                                            .ThenInclude(r => r.TopCard)
                                        .Include(g => g.Round)
                                            .ThenInclude(r => r.CardsInPlay)
                                        .Include(g => g.Players)
                                        .Include(g => g.Round)
                                        .Include(g => g.Players),
                        noTracking: false) ?? throw new NotImplementedException();

        return game;
    }

    private T DeserializeContent<T>(object actionData)
    {
        if (actionData is JsonElement jsonElement)
        {
            var result = JsonSerializer.Deserialize<T>(jsonElement.GetRawText())
                ?? throw new NotImplementedException();

            return result;
        }

        throw new NotImplementedException();
    }
    private async Task<GameState> CreateGame(PlayerAction action)
    {
        if (action.ActionData == null)
        {
            throw new NotImplementedException();
        }
        var createGameDto = DeserializeContent<CreateGameDto>(action.ActionData);

        var defaultGame = new Game()
        {
            PlayerQuantity = createGameDto.RequestedQuantity,
            Password = createGameDto.Password,
            Clockwise = true
        };
        var defaultPlayer = new Player()
        {
            Username = createGameDto.PlayerName,
            ConnectionId = action.ConnectionId,
            IsOwner = true,
            Position = 0,
        };

        defaultGame.Players.Add(defaultPlayer);

        var game = await _unitOfWork.Games.CreateAsync(defaultGame);
        await _unitOfWork.SaveAsync();

        var state = await GetGameState(game.Id);

        return state;
    }

    private async Task<GameState> JoinGame(PlayerAction action)
    {
        if (action.ActionData == null)
        {
            throw new NotImplementedException();
        }

        // Inputs of Join Game
        var joinGameDto = DeserializeContent<JoinGameDto>(action.ActionData);
        var gameId = joinGameDto.GameId;
        var username = joinGameDto.Username;
        var connectionId = action.ConnectionId;

        // Logic of Join Game
        var position = await _playerService.GetHighestPosition(gameId);

        var player = new Player()
        {
            Username = username,
            ConnectionId = connectionId,
            Position = position + 1,
            GameId = gameId
        };
        await _playerService.AddPlayerAsync(player);

        var players = (await _playerService.GetPlayersBy(gameId)).ToList();
        var playersQuantity = await _gameService.GetPlayersQuantity(gameId);
        
        if (players.Count() == playersQuantity)
        {
            var round = new Round()
            {
                GameId = gameId,
                Number = 1
            };

            await _roundService.CreateRound(round);
            await _roundService.FillTheCardsInPlayAsync(round.Id);
            
            await _gameService.UpdateRoundId(gameId, round.Id);

            var cardsInPlay = (await _roundService.GetCardsInRound(round.Id)).OrderBy(c => c.Position).Select(c => c.CardId).ToList();
            await _playerCardService.GiveCardsToPlayers(players, cardsInPlay, round.Id);
            
            var roundCards = await _roundService.GetCardsInRound(round.Id);
            var topCard = roundCards.OrderBy(r => r.Position).First().Card;
            
            while (CheckForWild(topCard.Value))
            {
                topCard = await _roundService.ShuffleUntilTopCardAsync(round.Id);
            }
            
            await _roundService.UpdateRoundTopCardAsync(round.Id, topCard);
            await _playerService.SetTurn(true, gameId, -1, 1, playersQuantity);
        }

        var state = await GetGameState(gameId);

        return state;
    }

    private bool CheckForWild(CardValue value)
    {
        return value == CardValue.Wild || value == CardValue.WildDrawFour;
    }

    private bool IsValidMove(Card topCard, PlayCardDto playedCard, CardColor? wildColor = null)
    {
        if (playedCard.CardValue == CardValue.Wild || playedCard.CardValue == CardValue.WildDrawFour)
        {
            return true;
        }

        if (wildColor != null && playedCard.CardColor == wildColor)
        {
            return true;
        }

        if (playedCard.CardColor == topCard.Color || playedCard.CardValue == topCard.Value)
        {
            return true;
        }

        if (playedCard.CardValue == CardValue.DrawTwo ||
            playedCard.CardValue == CardValue.Skip ||
            playedCard.CardValue == CardValue.Reverse)
        {
            return playedCard.CardValue == topCard.Value;
        }

        return false;
    }
    
    private async Task<GameState> PlayCard(PlayerAction action)
    {
        if (action.ActionData == null)
        {
            throw new NotImplementedException();
        }
        
        // Inputs of play card
        var playedCard = DeserializeContent<PlayCardDto>(action.ActionData);
        var gameId = action.GameId;
        
        var round = await _gameService.GetRound(gameId);
        var topCard = round.TopCard;

        var player = await _playerService.GetTurn(gameId);
        var quantity = await _gameService.GetPlayersQuantity(gameId);
        
        var transaction = await _unitOfWork.BeginTransactionAsync();

        CardColor? wildColor = null;
        if (playedCard.CardValue == CardValue.Wild)
        {
            await _roundService.PlayedWildCardAsync(round.Id, playedCard);
        }

        wildColor = await _roundService.CheckForWildTopCardAsync(round.Id);
        var card = await _unitOfWork.Cards.ReadOneAsync(
            predicate: c => c.Color == playedCard.CardColor && c.Value == playedCard.CardValue && c.Id != topCard.Id, noTracking: true);
        
        if (IsValidMove(topCard, playedCard, wildColor))
        {
            await _roundService.UpdateRoundTopCardAsync(round.Id, card);
            
            await _playerCardService.RemovePlayerCardAsync(player.Id, playedCard);
            
            bool direction = await _gameService.GetDirection(gameId ?? throw new InvalidOperationException());

            Player? nextPlayer;
            if (card.Value == CardValue.Skip)
            {
                nextPlayer = await _playerService.SetTurn(direction, gameId ?? throw new InvalidOperationException(), player.Position, 2, quantity);
            }
            else if (card.Value == CardValue.Reverse)
            {
                direction = await _gameService.ChangeDirection(gameId ?? throw new InvalidOperationException());
                nextPlayer = await _playerService.SetTurn(direction, gameId ?? throw new InvalidOperationException(),
                    player.Position, 1, quantity);
            }
            else if (card.Value == CardValue.DrawTwo)
            {
                var p = await _playerService.GetNextPlayer(direction, player.Position, gameId, quantity);
                await _playerCardService.GivePlayerTwoCards(p.Id, round.Id);
                nextPlayer = await _playerService.SetTurn(direction, gameId ?? throw new InvalidOperationException(),
                    player.Position, 2, quantity);
            }
            else if (card.Value == CardValue.WildDrawFour)
            {
                var p = await _playerService.GetNextPlayer(direction, player.Position, gameId, quantity);
                await _playerCardService.GivePlayerTwoCards(p.Id, round.Id);
                await _playerCardService.GivePlayerTwoCards(p.Id, round.Id);
                nextPlayer = await _playerService.SetTurn(direction, gameId ?? throw new InvalidOperationException(),
                    player.Position, 2, quantity);
            }
            else
            {
                nextPlayer = await _playerService.SetTurn(direction, gameId ?? throw new InvalidOperationException(),
                    player.Position, 1, quantity);
            }

            if (playedCard.CardValue != CardValue.Wild || playedCard.CardValue != CardValue.WildDrawFour)
            {
                await _roundService.RemoveTopCardWildColor(round.Id);
            }

            if (nextPlayer != null)
            {
                await _playerService.ChangePlayerTurnAsync(player);
            }
        }

        if (await _playerCardService.GetCardsQuantity(player.Id) == 0)
        {
            var score = await _playerCardService.AccumulateScoresAsync(gameId, player.Id);
            await _playerService.UpdatePlayerScoreAsync(player, score);
            if (score >= 500)
            {
                await DeclareWinner(player, gameId);
            }
            else
            {
                await NewRoundStarted(gameId);
            }
        }
        
        try
        {
            await _unitOfWork.SaveAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        var gameState = await GetGameState(gameId ?? throw new NotImplementedException());

        return gameState;
    }

    private async Task DeclareWinner(Player player, Guid? gameId)
    {
        await _gameService.WinGameAsync(player, gameId);
    }

    private async Task NewRoundStarted(Guid? gameId)
    {
        var round = await _gameService.GetRound(gameId);
        var players = (await _playerService.GetPlayersBy(gameId ?? throw new InvalidOperationException())).ToList();
        await _gameService.NewRoundStartedAsync(round);
        await _playerCardService.DeletePlayerCardsAsync(players.Select(p => p.Id));
        await _roundService.DeleteCardsInPlayAsync(round.Id);
        await _roundService.FillTheCardsInPlayAsync(round.Id);
        
        var cardsInPlay = (await _roundService.GetCardsInRound(round.Id)).OrderBy(c => c.Position).Select(c => c.CardId).ToList();
        await _playerCardService.GiveCardsToPlayers(players, cardsInPlay, round.Id);
        
        var roundCards = await _roundService.GetCardsInRound(round.Id);
        var topCard = roundCards.OrderBy(r => r.Position).First().Card;
            
        while (CheckForWild(topCard.Value))
        {
            topCard = await _roundService.ShuffleUntilTopCardAsync(round.Id);
        }
            
        await _roundService.UpdateRoundTopCardAsync(round.Id, topCard);
        await _playerService.SetTurn(true, gameId ?? throw new InvalidOperationException(), -1, 1, players.Count);
    }

    private async Task<GameState> DrawCard(PlayerAction action)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync();
        var player = await _playerService.GetTurn(action.GameId);

        if (action.ActionData != null)
        {
            var userName = DeserializeContent<string>(action.ActionData);
            
            if (player.Username != userName)
            {
                return await GetGameState(action.GameId ?? throw new NotImplementedException());
            }
        }
        
        var round = await _gameService.GetRound(action.GameId);
        var quantity = await _gameService.GetPlayersQuantity(action.GameId);
        
        var cardsInPlay = await _roundService.GetCardsInRound(round.Id);
        if (cardsInPlay.Count == 0)
        {
            var playerCardIds = await _playerCardService.GetCardsInPlayAsync(action.GameId);
            await _roundService.ReshuffleDiscardPilesAsync(round.Id, playerCardIds);
        }
        var drawnCard = cardsInPlay.First();
        await _playerCardService.GiveCardToPlayer(player.Id, drawnCard.CardId);
        await _roundService.RemoveCardInRoundAsync(round.Id, drawnCard.CardId);

        if (!IsValidMove(round.TopCard, new PlayCardDto()
            {
                CardColor = drawnCard.Card.Color,
                CardValue = drawnCard.Card.Value
            }))
        {
            await _playerService.ChangePlayerTurnAsync(player);
            bool direction = await _gameService.GetDirection(action.GameId ?? throw new InvalidOperationException());
            await _playerService.SetTurn(direction, action.GameId ?? throw new InvalidOperationException(), player.Position,
                1, quantity);
        }
        
        try
        {
            await _unitOfWork.SaveAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }

        var gameState = await GetGameState(action.GameId ?? throw new NotImplementedException());

        return gameState;
    }

    private async Task<GameState> PlayedDrawTwo(PlayerAction action)
    {
        var playerWithTurn = await _unitOfWork.Players.ReadOneAsync(
            predicate: player => player.ConnectionId == action.ConnectionId);

        var game = await _unitOfWork.Games.ReadByIdAsync(action.GameId);

        var drawStack = 2;
        if (playerWithTurn.DrawStack != 0)
        {
            drawStack += playerWithTurn.DrawStack;
            playerWithTurn.DrawStack = 0;
        }

        playerWithTurn.IsTurn = false;
        var player = await _unitOfWork.Players.ReadOneAsync(
            predicate: player => player.Position == (playerWithTurn.Position + 1) % game.PlayerQuantity);
        player.IsTurn = true;
        player.DrawStack = drawStack;

        var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.SaveAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }

        var gameState = await GetGameState(action.GameId ?? throw new NotImplementedException());

        return gameState;
    }

    private async Task<GameState> PlayedDrawFour(PlayerAction action)
    {
        var playerWithTurn = await _unitOfWork.Players.ReadOneAsync(
            predicate: player => player.ConnectionId == action.ConnectionId);

        var game = await _unitOfWork.Games.ReadByIdAsync(action.GameId);

        var drawStack = 4;
        if (playerWithTurn.DrawStack != 0)
        {
            drawStack += playerWithTurn.DrawStack;
            playerWithTurn.DrawStack = 0;
        }

        playerWithTurn.IsTurn = false;
        var player = await _unitOfWork.Players.ReadOneAsync(
            predicate: player => player.Position == (playerWithTurn.Position + 1) % game.PlayerQuantity);
        player.IsTurn = true;
        player.DrawStack = drawStack;

        var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.SaveAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }

        var gameState = await GetGameState(action.GameId ?? throw new NotImplementedException());

        return gameState;
    }

    private Task<GameState> ChallengeWildDrawFour(PlayerAction action)
    {
        throw new NotImplementedException();
    }

    private async Task<GameState> UnoDeclaration(PlayerAction action)
    {
        //TODO: Check if one can say uno
        var player = await _playerService.GetTurn(action.GameId);

        await _playerService.SaidUnoAsync(player);
        
        var gameState = await GetGameState(action.GameId ?? throw new NotImplementedException());

        return gameState;
    }

    private Task<GameState> WinGame(PlayerAction action)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<GameStateChange> GetGameStateChanges(GameState? oldState, GameState newState)
    {
        var changes = new List<GameStateChange>();

        if (oldState == null)
        {
            changes.Add(new GameStateChange
            {
                Type = "GameCreated",
                Data = new GameCreatedDto
                {
                    GameId = newState.Game.Id,
                    Players = newState.Game.Players.Select(p => new PlayerState
                    {
                        Username = p.Username,
                        GameId = newState.Game.Id
                    }).ToList()
                }
            });

            return changes;
        }

        if (oldState.Game.Players.Count != newState.Game.Players.Count)
        {
            changes.Add(new GameStateChange
            {
                Type = "GamePlayersChanged",
                Data = newState.Game.Players.Select(p => new PlayerState
                {
                    ConnectionId = p.ConnectionId,
                    Score = p.Score,
                    Username = p.Username,
                    UnoSaid = p.UnoSaid,
                    Cards = newState.PlayerHands.First(ph => ph.PlayerId == p.Id).Cards,
                    GameId = newState.Game.Id,
                }).ToList()
            });
        }

        if (oldState.Game.Round == null || oldState.Game.Round.TopCard != newState.Game.Round.TopCard)
        {
            changes.Add(new GameStateChange
            {
                Type = "TopCardChanged",
                Data = new GameTopCardDto
                {
                    GameId = newState.Game.Id,
                    Card = new Shared.DTOs.CardDto
                    {
                        Color = newState.Game.Round.TopCard.Color,
                        Value = newState.Game.Round.TopCard.Value,
                    }
                }
            });
        }

        foreach (var newPlayerState in newState.Game.Players)
        {
            var player = oldState.Game.Players.FirstOrDefault(p => p.Id == newPlayerState.Id);

            if (player == null || !newState.PlayerHands.First(ph => ph.PlayerId == newPlayerState.Id).Cards.Equals(oldState.PlayerHands.First(ph => ph.PlayerId == player.Id).Cards))
            {
                changes.Add(new GameStateChange
                {
                    Type = "PlayerCardsChanged",
                    Data = new GamePlayerCards
                    {
                        GameId = newState.Game.Id,
                        Cards = newState.PlayerHands.First(ph => ph.PlayerId == newPlayerState.Id).Cards,
                        ConnectionId = newPlayerState.ConnectionId
                    }
                });
            }

            if (player != null && newPlayerState.Score != player.Score)
            {
                changes.Add(new GameStateChange()
                {
                    Type = "PlayerScoreChanged",
                    Data = new GameScoreDto
                    {
                        ConnectionId = newPlayerState.ConnectionId,
                        Score = newPlayerState.Score,
                    }
                });
            }

            if (newPlayerState.IsTurn == true)
            {
                changes.Add(new GameStateChange()
                {
                    Type = "ChangedTurn",
                    Data = new GameTurnDto
                    {
                        ConnectionId = newPlayerState.ConnectionId,
                        Username = newPlayerState.Username,
                        GameId = newState.Game.Id
                    }
                });
            }
        }
        
        if (oldState.Game.Round == null || oldState.Game.Round.Number != newState.Game.Round.Number) 
        {
                changes.Add(new GameStateChange
                {
                    Type = "NewRound",
                    Data = new GameRoundDto
                    {
                        GameId = newState.Game.Id,
                        Round = newState.Game.Round.Number + 1
                    }
                }); 
        }

        /*
            if (newState.Game.WinnerName != null)
            {
                changes.Add(new GameStateChange
                {
                    Type = "WonGame",
                    Data = new GameWinnerDto
                    {
                        GameId = newState.Game.Id,
                        ConnectionId = newState.Game.Players.First(p => p.Username == newState.Game.WinnerName).ConnectionId,
                        WinnerName = newState.Game.WinnerName,
                    }
                });
            }
            */

        return changes;
    }
}
