using Microsoft.EntityFrameworkCore.Storage;
using UnoAccess.Context;
using UnoAccess.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    public ICardRepository CardRepository { get; }
    public IDeckRepository DeckRepository { get; }
    public IGameRepository GameRepository { get; }
    public IPlayerRepository PlayerRepository { get; }
    
    public ICardDeckRepository CardDeckRepository { get; }
    public IDiscardPileDeckRepository DiscardPileDeckRepository { get; }
    public ICardPlayerRepository CardPlayerRepository { get; }

    public UnitOfWork(
        ICardRepository cardRepository,
        IDeckRepository deckRepository,
        IGameRepository gameRepository,
        IPlayerRepository playerRepository,
        ICardDeckRepository cardDeckRepository,
        IDiscardPileDeckRepository discardPileDeckRepository,
        ICardPlayerRepository cardPlayerRepository)
    {
        CardRepository = cardRepository;
        DeckRepository = deckRepository;
        GameRepository = gameRepository;
        PlayerRepository = playerRepository;
        CardDeckRepository = cardDeckRepository;
        DiscardPileDeckRepository = discardPileDeckRepository;
        CardPlayerRepository = cardPlayerRepository;
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
