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
    public async Task RemovePlayerCardAsync(Guid cardId, Guid playerId)
    {
        await _unitOfWork.PlayerCards.RemovePlayerCard(cardId, playerId);

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
}