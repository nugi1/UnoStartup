using Shared.DTOs;
using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoServices.Interfaces;

namespace UnoServices.Services;

public class PlayerService : IPlayerService
{
    private readonly IUnitOfWork _unitOfWork;
    public PlayerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<List<Player>> AddPlayers(Guid gameId, List<Player> players)
    {
        foreach (var player in players)
        {
            player.GameId = gameId;
            await _unitOfWork.PlayerRepository.CreatePlayerAsync(player);
        }

        return players;
    }

    public async Task<Player> SayUno(Guid playerId)
    {
        var player = await _unitOfWork.PlayerRepository.GetPlayerByIdAsync(playerId);
        player.UnoSaid = true;

        return player;
    }

    public async Task<Player> SayUnoTill(Guid playerId)
    {
        var player = await _unitOfWork.PlayerRepository.GetPlayerByIdAsync(playerId);
        player.SayUnoUntil = DateTime.Now.AddSeconds(30);

        _unitOfWork.PlayerRepository.UpdatePlayerAsync(player);

        return player;
    }

    public async Task<Player> GetPlayerById(Guid playerId)
    {
        var player = await _unitOfWork.PlayerRepository.GetPlayerByIdAsync(playerId);

        return player;
    }

    public async Task<int> CountScore(Guid playerId, Guid deckId)
    {
        var players = await _unitOfWork.PlayerRepository.GetAllPlayers(deckId.ToString());

        int score = 0;
        foreach (var player in players)
        {
            var cards = await _unitOfWork.CardPlayerRepository.GetPlayerCards(playerId);
            foreach (var card in cards)
            {
                player.Score += await GetCardValue(card.CardId);
            }

            _unitOfWork.PlayerRepository.UpdatePlayerAsync(player);
        }

        return score;
    }

    private async Task<int> GetCardValue(Guid cardId)
    {
        var card = (await _unitOfWork.CardRepository.GetAllCardsAsync()).FirstOrDefault(c => c.Id == cardId);
        
        return card.Value switch
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
            CardValue.Skip => 20,
            CardValue.Reverse => 20,
            CardValue.DrawTwo => 20,
            CardValue.Wild => 50,
            CardValue.WildDrawFour => 50,
            _ => 0
        };
    }
}