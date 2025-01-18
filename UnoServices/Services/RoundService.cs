using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Actions;
using Shared.Enums;
using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoServices.Interfaces;

namespace UnoServices.Services;

public class RoundService(IUnitOfWork unitOfWork) : IRoundService
{
    public async Task UpdateRoundTopCardAsync(Guid? roundId, Card topCard)
    {
        if (roundId == null)
        {
            throw new InvalidOperationException();
        }
        var round = await unitOfWork.Rounds.ReadByIdAsync(roundId) 
                    ?? throw new InvalidOperationException($"Round does not exist with Id: {roundId}");
        
        round.TopCardId = topCard.Id;
        round.TopCard = topCard;
        
        await unitOfWork.RoundCards.DeleteBy(round.Id, topCard.Id);
        unitOfWork.Rounds.Update(round);
        
        await unitOfWork.SaveAsync();
    }

    public async Task FillTheCardsInPlayAsync(Guid roundId)
    {
        var cards = await unitOfWork.Cards.ReadAsync();
        var roundCards = ShuffleCards(cards, roundId);

        foreach (var card in roundCards)
        {
            await unitOfWork.RoundCards.CreateAsync(card);
        }

        await unitOfWork.SaveAsync();
    }

    public async Task RemoveCardInRoundAsync(Guid roundId, Guid cardId)
    {
        await unitOfWork.RoundCards.DeleteBy(roundId, cardId);

        await unitOfWork.SaveAsync();
    }

    public async Task CreateRound(Round round)
    {
        await unitOfWork.Rounds.CreateAsync(round);

        await unitOfWork.SaveAsync();
    }

    public async Task<Card> ShuffleUntilTopCardAsync(Guid roundId)
    {
        var cards = await unitOfWork.RoundCards.ReadAsync(
            predicate: rc => rc.RoundId == roundId,
            include: rc => rc.Include(r => r.Card));
        var sCards = cards.Select(c => c.Card).ToList();
        var newRoundCards = ShuffleCards(sCards, roundId);
        
        foreach (var card in newRoundCards)
        {
            var entity = unitOfWork.RoundCards.GetTrackedEntity(card.RoundId, card.CardId);
            if (entity != null)
            {
                unitOfWork.RoundCards.DetachEntity(entity);
            }
            unitOfWork.RoundCards.Update(card);
        }

        await unitOfWork.SaveAsync();
        
        return newRoundCards.OrderBy(n => n.Position).Select(n => n.Card).First();
    }

    public async Task DeleteCardsInPlayAsync(Guid roundId)
    {
        var cardsInPlay = await unitOfWork.RoundCards.ReadAsync(
            predicate: rc => rc.RoundId == roundId);

        foreach (var cards in cardsInPlay)
        {
            await unitOfWork.RoundCards.DeleteBy(cards.RoundId, cards.CardId);
        }

        await unitOfWork.SaveAsync();
    }

    public async Task ReshuffleDiscardPilesAsync(Guid roundId, IEnumerable<Guid> playerCardIds)
    {
        var cards = await unitOfWork.Cards.ReadAsync();
        var roundCards = ShuffleCards(cards, roundId);

        foreach (var card in roundCards)
        {
            if (!playerCardIds.Contains(card.CardId))
            {
                await unitOfWork.RoundCards.CreateAsync(card);
            }
        }

        await unitOfWork.SaveAsync();    
    }

    public async Task<IList<RoundCard>> GetCardsInRound(Guid roundId)
    {
        var round = await unitOfWork.RoundCards.ReadAsync(
            predicate: rc => rc.RoundId == roundId,
            include: rc => rc.Include(c => c.Card),
            noTracking: true);

        return round;
    }

    public async Task PlayedWildCardAsync(Guid roundId, PlayCardDto playedCard)
    {
        var round = await unitOfWork.Rounds.ReadByIdAsync(roundId);
        round!.WildColor = playedCard.CardColor;

        unitOfWork.Rounds.Update(round);
        await unitOfWork.SaveAsync();
    }
    public async Task<CardColor?> CheckForWildTopCardAsync(Guid roundId)
    {
        var round = await unitOfWork.Rounds.ReadByIdAsync(roundId);
        return round!.WildColor;
    }
    public async Task RemoveTopCardWildColor(Guid roundId)
    {
        var round = await unitOfWork.Rounds.ReadByIdAsync(roundId);
        round!.WildColor = null;
        unitOfWork.Rounds.Update(round);
        await unitOfWork.SaveAsync();
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