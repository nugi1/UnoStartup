using Shared.DTOs;
using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoAccess.PredefinedData;
using UnoServices.Interfaces;

namespace UnoServices.Services;

public class DeckService : IDeckService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeckService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Deck> CreateDeck(CreateDeckDto deckDto)
    {
        var deck = new Deck()
        {
            DeckName = deckDto.OwnerPlayer,
            IsPrivate = deckDto.IsPrivate,
            Till = DateTime.Now.AddMilliseconds(deckDto.MaxWaitTime).ToUniversalTime(),
            PlayersCount = deckDto.RequestedQuantity
        };

        await _unitOfWork.DeckRepository.CreateDeckAsync(deck);
        
        return deck;
    }

    public async Task<Deck?> ReadDeck(Guid id)
    {
        var deck = await _unitOfWork.DeckRepository.GetDeckByIdAsync(id);

        return deck;
    }

    public async Task<Player> GetPlayerWithTurn(Guid deckId)
    {
        var player = await _unitOfWork.DeckRepository.GetTurn(deckId);

        return player;
    }

    public async Task<Player> SetTurn(int position, Guid deckId)
    {
        var players = (await _unitOfWork.PlayerRepository.GetAllPlayers(deckId.ToString())).ToList();
        var deck = (await _unitOfWork.DeckRepository.GetDeckByIdAsync(deckId));
        deck.CurrentPlayerId = players[position % players.Count].Id;

        await _unitOfWork.DeckRepository.UpdateDeckAsync(deck);

        return players[position % players.Count];
    }

    public async Task TogglePlayDirection(Guid deckId)
    {
        var players = (await _unitOfWork.PlayerRepository.GetAllPlayers(deckId.ToString())).ToList();

        for (int i = 0; i < players.Count / 2; i++)
        {
            (players[players.Count - i - 1].Position, players[i].Position) =
                (players[i].Position, players[players.Count - i - 1].Position);

            _unitOfWork.PlayerRepository.UpdatePlayerAsync(players[i]);
        }
    }

    public async Task<Player> SetScoreAsync(Guid playerId, Guid deckId, int score)
    {
        var player = await _unitOfWork.PlayerRepository.GetPlayerByIdAsync(playerId);

        player.Score = score;
        _unitOfWork.PlayerRepository.UpdatePlayerAsync(player);
        
        return player;
    }

    public async Task RefreshDeck(Guid deckId)
    {
        var cardDecks = await _unitOfWork.CardDeckRepository.GetCardsByDeck(deckId);

        foreach (var cardDeck in cardDecks)
        {
            await _unitOfWork.CardDeckRepository.RemoveCardAsync(cardDeck);
        }

        var players = await _unitOfWork.PlayerRepository.GetAllPlayers(deckId.ToString());

        foreach (var player in players)
        {
            var cardPlayers = await _unitOfWork.CardPlayerRepository.GetPlayerCards(player.Id);

            foreach (var cardPlayer in cardPlayers)
            {
                await _unitOfWork.CardPlayerRepository.RemoveAsync(cardPlayer);
            }
        }
        
        await _unitOfWork.DiscardPileDeckRepository.RemoveDiscardPiles(deckId);
    }
}