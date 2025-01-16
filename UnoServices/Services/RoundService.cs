using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Actions;
using Shared.Enums;
using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoServices.Interfaces;

namespace UnoServices.Services;

public class RoundService : IRoundService
{
    private readonly IUnitOfWork _unitOfWork;
    public RoundService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task UpdateRoundTopCardAsync(Guid? roundId, Card topCard)
    {
        if (roundId == null)
        {
            throw new InvalidOperationException();
        }
        var round = await _unitOfWork.Rounds.ReadByIdAsync(roundId) 
                    ?? throw new InvalidOperationException($"Round does not exist with Id: {roundId}");
        var roundCard = await _unitOfWork.RoundCards.ReadOneAsync(
            predicate: r => r.CardId == topCard.Id && r.RoundId == roundId);
        
        round.TopCardId = topCard.Id;
        round.TopCard = topCard;
        
        await _unitOfWork.RoundCards.DeleteBy(round.Id, topCard.Id);
        _unitOfWork.Rounds.Update(round);
        
        await _unitOfWork.SaveAsync();
    }

    public async Task FillTheCardsInPlayAsync(Guid roundId)
    {
        var cards = await _unitOfWork.Cards.ReadAsync();
        var roundCards = ShuffleCards(cards, roundId);

        foreach (var card in roundCards)
        {
            await _unitOfWork.RoundCards.CreateAsync(card);
        }

        await _unitOfWork.SaveAsync();
    }

    public async Task RemoveCardInRoundAsync(Guid roundId, Guid cardId)
    {
        await _unitOfWork.RoundCards.DeleteBy(roundId, cardId);

        await _unitOfWork.SaveAsync();
    }

    public async Task CreateRound(Round round)
    {
        await _unitOfWork.Rounds.CreateAsync(round);

        await _unitOfWork.SaveAsync();
    }

    public async Task<Card> ShuffleUntilTopCardAsync(Guid roundId)
    {
        var cards = await _unitOfWork.RoundCards.ReadAsync(
            predicate: rc => rc.RoundId == roundId,
            include: rc => rc.Include(r => r.Card));
        var sCards = cards.Select(c => c.Card).ToList();
        var newRoundCards = ShuffleCards(sCards, roundId);
        
        foreach (var card in newRoundCards)
        {
            _unitOfWork.RoundCards.Update(card);
        }

        await _unitOfWork.SaveAsync();
        
        return newRoundCards.OrderBy(n => n.Position).Select(n => n.Card).First();
    }

    public async Task DeleteCardsInPlayAsync(Guid roundId)
    {
        var cardsInPlay = await _unitOfWork.RoundCards.ReadAsync(
            predicate: rc => rc.RoundId == roundId);

        foreach (var cards in cardsInPlay)
        {
            await _unitOfWork.RoundCards.DeleteBy(cards.RoundId, cards.CardId);
        }

        await _unitOfWork.SaveAsync();
    }

    public async Task ReshuffleDiscardPilesAsync(Guid roundId, IEnumerable<Guid> playerCardIds)
    {
        var cards = await _unitOfWork.Cards.ReadAsync();
        var roundCards = ShuffleCards(cards, roundId);

        foreach (var card in roundCards)
        {
            if (!playerCardIds.Contains(card.CardId))
            {
                await _unitOfWork.RoundCards.CreateAsync(card);
            }
        }

        await _unitOfWork.SaveAsync();    
    }

    public async Task PlayedWildCardAsync(Guid roundId, PlayCardDto playedCard)
    {
        var round = await _unitOfWork.Rounds.ReadByIdAsync(roundId);

        round.WildColor = playedCard.CardColor;
        
        _unitOfWork.Rounds.Update(round);
        await _unitOfWork.SaveAsync();
    }

    public async Task<CardColor?> CheckForWildTopCardAsync(Guid roundId)
    {
        var round = await _unitOfWork.Rounds.ReadByIdAsync(roundId);

        return round.WildColor;
    }

    public async Task RemoveTopCardWildColor(Guid roundId)
    {
        var round = await _unitOfWork.Rounds.ReadByIdAsync(roundId);
        round.WildColor = null;

        _unitOfWork.Rounds.Update(round);
        await _unitOfWork.SaveAsync();
    }

    public async Task<IList<RoundCard>> GetCardsInRound(Guid roundId)
    {
        var round = await _unitOfWork.RoundCards.ReadAsync(
            predicate: rc => rc.RoundId == roundId,
            include: rc => rc.Include(c => c.Card),
            noTracking: true);

        return round;
    }
    
    private List<RoundCard> ShuffleCards(IList<Card> cards, Guid roundId)
    {
        var result = new List<RoundCard>();
        var random = new Random();
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);

            (cards[i], cards[j]) = (cards[j], cards[i]);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            result.Add(new RoundCard()
            {
                CardId = cards[i].Id,
                Position = i + 1,
                RoundId = roundId
            });
        }

        return result;
    }
}