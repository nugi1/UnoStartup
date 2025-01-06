namespace UnoAccess.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICardRepository CardRepository { get; }
    IDeckRepository DeckRepository { get; }
    IGameRepository GameRepository { get; }
    IPlayerRepository PlayerRepository { get; }
    ICardDeckRepository CardDeckRepository { get; }
    public IDiscardPileDeckRepository DiscardPileDeckRepository { get; }
    public ICardPlayerRepository CardPlayerRepository { get; }
}