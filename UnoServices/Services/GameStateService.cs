using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
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
        var act = _actionDelegates.Where(ad => ad.ActionName == action.Userame).FirstOrDefault();

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
            Round = game.Round,
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

    private async Task<GameState> CreateGame(PlayerAction action)
    {
        var defaultGame = new Game();
        var defaultPlayer = new Player()
        {
            Username = action.Userame
        };
        
        defaultGame.Players.Add(defaultPlayer);

        var game = await _unitOfWork.Games.CreateAsync(defaultGame);

        var state = await GetGameState(game.Id);

        return state;
    }

    private async Task<GameState> JoinGame(PlayerAction action)
    {
        if (action.GameId != null)
        {
            throw new NotImplementedException();
        }

        var player = new Player()
        {
            Username = action.Userame
        };
        var game = await GetGame(action.GameId ?? throw new NotImplementedException());

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

                game.Players[i % 7].Cards.Add(new Card()
                {
                    Id = card.Id,
                    Color = card.Color,
                    Value = card.Value,
                });
            }

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

    private Task<GameState> PlayCard(PlayerAction action)
    {
        throw new NotImplementedException();
    }

    private Task<GameState> SkipTurn(PlayerAction action)
    {
        throw new NotImplementedException();
    }

    private Task<GameState> ReverseTurn(PlayerAction action)
    {
        throw new NotImplementedException();
    }

    private Task<GameState> DrawCard(PlayerAction action)
    {
        throw new NotImplementedException();
    }

    private Task<GameState> PlayedDrawTwo(PlayerAction action)
    {
        throw new NotImplementedException();
    }

    private Task<GameState> PlayedDrawFour(PlayerAction action)
    {
        throw new NotImplementedException();
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
        var cards = await _unitOfWork.Cards.ReadAsync();
        var roundCards = cards.Select(c => new RoundCard
        {
            CardId = c.Id,
            RoundId = roundId,
        }).ToList();

        var random = new Random();
        for (int i = roundCards.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);

            var temp = roundCards[i];
            roundCards[i] = roundCards[j];
            roundCards[j] = temp;
        }

        for (int i = 0; i < roundCards.Count; i++)
        {
            roundCards[i].Position = i + 1;
        }

        return roundCards;
    }
}
