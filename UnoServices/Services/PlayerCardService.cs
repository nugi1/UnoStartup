using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Actions;
using Shared.Enums;
using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoServices.Interfaces;

namespace UnoServices.Services;

public class PlayerCardService : IPlayerCardService
{
    private readonly IUnitOfWork _unitOfWork;
    public PlayerCardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task RemovePlayerCardAsync(Guid playerId, PlayCardDto playedCard)
    {
        var card = (await _unitOfWork.PlayerCards.GetPlayerCards(playerId)).First(
            pc => (pc.Color == playedCard.CardColor && pc.Value == playedCard.CardValue) || (playedCard.CardValue == CardValue.Wild && pc.Value == CardValue.Wild) || (playedCard.CardValue == CardValue.WildDrawFour && pc.Value == CardValue.WildDrawFour));

        await _unitOfWork.PlayerCards.RemovePlayerCard(card.Id, playerId);
        await _unitOfWork.SaveAsync();
    }

    public async Task AddPlayerCardsAsync(PlayerCard pCard)
    {
        await _unitOfWork.PlayerCards.AddPlayerCard(pCard);

        await _unitOfWork.SaveAsync();
    }

    public async Task GiveCardsToPlayers(IList<Player> players, IList<Guid> cardsInPlay, Guid roundId)
    {
        var playersQuantity = players.Count;
        
        for (int i = 0; i < 7 * playersQuantity; i++)
        {
            var player = players[i % playersQuantity];
            var card = cardsInPlay.First();

            var pCard = new PlayerCard()
            {
                PlayerId = player.Id,
                CardId = card
            };

            await _unitOfWork.PlayerCards.AddPlayerCard(pCard);
            await _unitOfWork.RoundCards.DeleteBy(roundId, card);
            cardsInPlay.RemoveAt(0);
        }

        await _unitOfWork.SaveAsync();
    }

    public async Task<bool> CardExistsForPlayer(Guid playerId, Guid cardId)
    {
        var exists = await _unitOfWork.PlayerCards.ReadOneAsync(
            predicate: p => p.PlayerId == playerId && p.CardId == cardId);

        return exists != null;
    }

    public async Task GiveCardToPlayer(Guid playerId, Guid cardId)
    {
        await _unitOfWork.PlayerCards.AddPlayerCard(new PlayerCard()
        {
            PlayerId = playerId,
            CardId = cardId
        });

        await _unitOfWork.SaveAsync();
    }

    public async Task GivePlayerTwoCards(Guid playerId, Guid roundId)
    {
        var cardsInPlay = await _unitOfWork.RoundCards.ReadAsync(
            predicate: c => c.RoundId == roundId);

        var cardsToGive = cardsInPlay.OrderBy(cp => cp.Position).Take(2);

        foreach (var card in cardsToGive)
        {
            await _unitOfWork.PlayerCards.AddPlayerCard(new PlayerCard()
            {
                CardId = card.CardId,
                PlayerId = playerId
            });

            await _unitOfWork.RoundCards.DeleteBy(roundId, card.CardId);
        }

        await _unitOfWork.SaveAsync();
    }

    public async Task<int> GetCardsQuantity(Guid playerId)
    {
        var playerCards = await _unitOfWork.PlayerCards.GetPlayerCards(playerId);

        return playerCards.Count;
    }

    public async Task<int> AccumulateScoresAsync(Guid? gameId, Guid playerId)
    {
        var players = await _unitOfWork.Players.ReadAsync(
            p => p.GameId == gameId && p.Id != playerId);

        int score = 0;
        foreach (var player in players)
        {
            var playerCards = await _unitOfWork.PlayerCards.GetPlayerCards(player.Id);
            foreach (var c in playerCards)
            {
                score += GetCardPoints(c.Value);
            }
        }

        return score;
    }

    public async Task DeletePlayerCardsAsync(IEnumerable<Guid> playerIds)
    {
        foreach (var playerId in playerIds)
        {
            var playerCards = await _unitOfWork.PlayerCards.GetPlayerCards(playerId);

            foreach (var card in playerCards)
            {
                await _unitOfWork.PlayerCards.RemovePlayerCard(card.Id, playerId);
            }
        }

        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<Guid>> GetCardsInPlayAsync(Guid? actionGameId)
    {
        var players = await _unitOfWork.Players.ReadAsync(
            predicate: p => p.GameId == actionGameId,
            noTracking:true);

        IEnumerable<Guid> result = new List<Guid>();
        foreach (var player in players)
        {
            var playerCards = (await _unitOfWork.PlayerCards.GetPlayerCards(player.Id)).Select(p => p.Id);

            result = result.Union(playerCards);
        }

        return result;
    }

    private int GetCardPoints(CardValue value)
    {
        return value switch
        {
            CardValue.Zero => 0,
            CardValue.One => 1,
            CardValue.Two => 2,
            CardValue.Three => 3,
            CardValue.Four => 4,
            CardValue.Five => 5,
            CardValue.Six => 6,
            CardValue.Seven => 7,
            CardValue.Eight => 8,
            CardValue.Nine => 9,
            CardValue.DrawTwo => 20,
            CardValue.Reverse => 20,
            CardValue.Skip => 20,
            CardValue.Wild => 50,
            CardValue.WildDrawFour => 50,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Unknown card value: {value}")
        };
    }

    public async Task<Card> GetCardFromPlayersHand(Guid id, PlayCardDto playedCard)
    {
        var pc = await _unitOfWork.PlayerCards.ReadOneAsync(
            predicate: pc => pc.PlayerId == id && 
            (
            (pc.Card.Color == playedCard.CardColor && pc.Card.Value == playedCard.CardValue) || 
            (playedCard.CardValue == CardValue.Wild && pc.Card.Value == CardValue.Wild) || (playedCard.CardValue == CardValue.WildDrawFour && pc.Card.Value == CardValue.WildDrawFour)
            ),
            include: pc => pc.Include(c => c.Card),
            noTracking: true);

        return pc.Card;
    }
}