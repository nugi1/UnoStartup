using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoServices.Interfaces;

namespace UnoServices.Services;

public class CardPlayerService : ICardPlayerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CardPlayerService> _logger;

    public CardPlayerService(IUnitOfWork unitOfWork, ILogger<CardPlayerService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task FillThePlayers(Guid deckId)
    {
        var cards = (await _unitOfWork.CardDeckRepository.GetCardsByDeck(deckId)).OrderBy(cd => cd.Position).Select(cd => cd.CardId).ToList();
        var playerIds = (await _unitOfWork.PlayerRepository.GetAllPlayers(deckId.ToString())).Select(p => p.Id).ToList();


        await _unitOfWork.CardPlayerRepository.GiveCardsToPlayers(deckId, cards, playerIds);
    }

    public async Task RemovePlayerCard(CardDto cardDto, Guid deckId, Guid PlayerId)
    {
        var card = await _unitOfWork.CardRepository.GetCardByColorAndValue(cardDto.Color, cardDto.Value);
        _logger.LogInformation($"{card.Color} {card.Value}");
        var cardPlayer = await _unitOfWork.CardPlayerRepository.GetCardPlayer(PlayerId, card.Id);
        await _unitOfWork.CardPlayerRepository.RemoveAsync(cardPlayer);
    }

    public async Task GiveCardsToPlayer(Guid deckId, Guid playerId, int count)
    {
        var cards = (await _unitOfWork.CardDeckRepository.GetCardsByDeck(deckId)).OrderBy(c => c.Position).Take(count).Select(c => c.CardId).ToList();

        await _unitOfWork.CardPlayerRepository.GiveCardToPlayer(deckId, playerId, cards);
    }

    public async Task<IEnumerable<CardPlayer>> GetPlayerCards(Guid playerId)
    {
        var playerCards = await _unitOfWork.CardPlayerRepository.GetPlayerCards(playerId);

        return playerCards;
    }

    public async Task Clear(IEnumerable<Guid> playerIds)
    {
        foreach (var playerId in playerIds)
        {
            var cards = await _unitOfWork.CardPlayerRepository.GetPlayerCards(playerId);

            foreach (var card in cards)
            {
                await _unitOfWork.CardPlayerRepository.RemoveAsync(card);
            }
        }
    }
}