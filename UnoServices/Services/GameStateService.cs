using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Actions;
using Shared.DTOs.ChangeStateData;
using Shared.Enums;
using Shared.States;
using System;
using System.Numerics;
using System.Text.Json;
using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoServices.Delegates;
using UnoServices.Interfaces;

namespace UnoServices.Services;

public class GameStateService : IGameStateService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly List<DelegateAction> _actionDelegates;

    public GameStateService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

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
                ActionName = "SkipTurn",
                Action = SkipTurn
            },
            new DelegateAction
            {
                ActionName = "ReverseTurn",
                Action = ReverseTurn
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
                                 .ThenInclude(p => p.Cards)

        ) ?? throw new InvalidOperationException();

        var result = new GameState()
        {
            GameId = gameId,
            Game = game,
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
                                        .Include(g => g.Players)
                                            .ThenInclude(p => p.Cards),
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
        };
        var defaultPlayer = new Player()
        {
            Username = createGameDto.PlayerName,
            ConnectionId = action.ConnectionId,
            IsOwner = true,
            Position = 1
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

        var joinGameDto = DeserializeContent<JoinGameDto>(action.ActionData);

        var position = (await _unitOfWork.Games
            .ReadOneAsync(predicate: g => g.Id == joinGameDto.GameId, include: g => g.Include(e => e.Players))
            ?? throw new NotImplementedException())
            .Players.OrderByDescending(p => p.Position)
            .First().Position + 1;

        var player = new Player()
        {
            Username = joinGameDto.Username,
            ConnectionId = action.ConnectionId,
            Position = position
        };
        var game = await GetGame(joinGameDto.GameId);

        game.Players.Add(player);
        if (game.Players.Count == game.PlayerQuantity)
        {
            var round = new Round()
            {
                GameId = game.Id,
            };

            round.CardsInPlay = (await ShuffleCards(round.Id)).OrderBy(cp => cp.Position).ToList();
            game.Round = round;

            for (int i = 0; i < 7 * game.PlayerQuantity; i++)
            {
                var cardId = round.CardsInPlay[i].CardId;
                var card = await _unitOfWork.Cards.ReadByIdAsync(cardId);

                round.CardsInPlay.Remove(round.CardsInPlay[i]);

                game.Players[i % game.PlayerQuantity].Cards.Add(card);
            }

            var topCardId = round.CardsInPlay.First().CardId;
            var topCard = await _unitOfWork.Cards.ReadByIdAsync(topCardId);

            while (CheckForWild(topCard.Value))
            {
                topCardId = (await ShuffleCards(round.Id)).OrderBy(cp => cp.Position).First().CardId;
                topCard = await _unitOfWork.Cards.ReadByIdAsync(topCardId);
            }

            round.CardsInPlay.Remove(round.CardsInPlay.First());
            round.TopCardId = topCardId;
            round.TopCard = topCard;

            var playerTurn = game.Players.OrderBy(p => p.Position).First();
            playerTurn.IsTurn = true;

            var transaction = await _unitOfWork.BeginTransactionAsync();
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
        }

        var state = await GetGameState(game.Id);

        return state;
    }

    private bool CheckForWild(CardValue value)
    {
        return value == CardValue.Wild || value == CardValue.WildDrawFour;
    }

    private bool IsValidMove(Card topCard, PlayCardDto playedCard)
    {
        if (playedCard.CardValue == CardValue.Wild || playedCard.CardValue == CardValue.WildDrawFour)
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
        var game = await _unitOfWork.Games.ReadOneAsync(
            predicate: g => g.Id == action.GameId,
            noTracking: true);

        var player = await _unitOfWork.Players.ReadOneAsync(
            predicate: p => p.ConnectionId == action.ConnectionId,
            include: p => p.Include(p => p.Cards).Include(p => p.Round).ThenInclude(r => r.TopCard));

        var nextPlayer = await _unitOfWork.Players.ReadOneAsync(
            predicate: p => p.Position == (player.Position + 1) % game.PlayerQuantity);

        var playedCard = DeserializeContent<PlayCardDto>(action.ActionData);

        if ((player.Round.TopCard.Value == CardValue.DrawTwo || player.Round.TopCard.Value == CardValue.WildDrawFour)
            && (playedCard.CardValue == CardValue.DrawTwo || playedCard.CardValue == CardValue.WildDrawFour))
        {
            player.DrawStack += 4;
            nextPlayer.DrawStack = player.DrawStack;
            player.DrawStack = 0;
            nextPlayer.IsTurn = true;
        }

        if (IsValidMove(player.Round.TopCard, playedCard))
        {
            var card = (await _unitOfWork.Cards.ReadOneAsync(
                predicate: c => c.Color == playedCard.CardColor && c.Value == playedCard.CardValue));
            player.Round.TopCard = card;
            player.Cards.Remove(card);
        }

        player.IsTurn = false;

        var transaction = await _unitOfWork.BeginTransactionAsync();
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

        var gameState = await GetGameState(player.Round.GameId);

        return gameState;
    }

    private async Task<GameState> SkipTurn(PlayerAction action)
    {
        var playerSkipped = await _unitOfWork.Players.ReadOneAsync(
            predicate: p => p.ConnectionId == action.ConnectionId);
        playerSkipped.IsTurn = false;

        var game = await _unitOfWork.Games.ReadOneAsync(
            predicate: g => g.Id == action.GameId,
            noTracking: true);

        var nowTurn = await _unitOfWork.Players.ReadOneAsync(
            predicate: p => p.Position == (playerSkipped.Position + 1) % game.PlayerQuantity);
        nowTurn.IsTurn = true;

        var transaction = await _unitOfWork.BeginTransactionAsync();
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

        var gameState = await GetGameState(action.GameId ?? throw new NotImplementedException());

        return gameState;
    }

    private async Task<GameState> ReverseTurn(PlayerAction action)
    {
        var player = (await _unitOfWork.Games.ReadOneAsync(
            predicate: g => g.Id == action.GameId,
            include: g => g.Include(e => e.Players)) ?? throw new NotImplementedException()).Players;

        for (int i = 0; i < player.Count; i++)
        {
            player[i].Position = (player.Count - player[i].Position + 1) % player.Count;
        }

        var nowPosition = player.Where(p => p.IsTurn).First();
        nowPosition.IsTurn = false;

        var newPosition = player.Where(p => p.Position == nowPosition.Position + 1).First();
        nowPosition.IsTurn = true;

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

    private async Task<GameState> DrawCard(PlayerAction action)
    {
        var player = await _unitOfWork.Players.ReadOneAsync(
            predicate: p => p.ConnectionId == action.ConnectionId,
            include: p => p.Include(e => e.Cards).Include(e => e.Round));

        var drawnCard = player.Round.CardsInPlay.OrderBy(cp => cp.Position).First();
        var card = await _unitOfWork.Cards.ReadByIdAsync(drawnCard.CardId);

        player.Round.CardsInPlay.Remove(drawnCard);
        player.Cards.Add(card);

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

    private Task<GameState> UnoDeclaration(PlayerAction action)
    {
        throw new NotImplementedException();
    }

    private Task<GameState> WinGame(PlayerAction action)
    {
        throw new NotImplementedException();
    }

    private async Task<List<RoundCard>> ShuffleCards(Guid roundId)
    {
        var cards = (await _unitOfWork.RoundCards.ReadAsync(
            predicate: rc => rc.RoundId == roundId)).ToList();

        if (cards.Count == 0)
        {
            cards = (await _unitOfWork.Cards.ReadAsync()).Select(c => new RoundCard()
            {
                CardId = c.Id,
                RoundId = roundId
            }).ToList();
        }


        var random = new Random();
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);

            var temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].Position = i + 1;
        }

        return cards;
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

        if (oldState.Game.Players != newState.Game.Players)
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
                    Cards = p.Cards.Select(c => new Shared.DTOs.CardDto
                    {
                        Color = c.Color,
                        Value = c.Value,
                    }).ToList(),
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

            if (player == null || newPlayerState.Cards != player.Cards)
            {
                changes.Add(new GameStateChange
                {
                    Type = "PlayerCardsChanged",
                    Data = new GamePlayerCards
                    {
                        GameId = newState.Game.Id,
                        Cards = newPlayerState.Cards.Select(c => new Shared.DTOs.CardDto
                        {
                            Color = c.Color,
                            Value = c.Value,
                        }).ToList(),
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

        if (oldState.Game.Round == null || oldState.Game.Round.Played != newState.Game.Round.Played)
            {
                changes.Add(new GameStateChange
                {
                    Type = "NewRound",
                    Data = new GameRoundDto
                    {
                        GameId = newState.Game.Id,
                        Round = newState.Game.Round.Played + 1
                    }
                });
            }

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

        return changes;
    }
}
