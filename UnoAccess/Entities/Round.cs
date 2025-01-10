namespace UnoAccess.Entities;

public class Round : Base
{
    public int Played { get; set; }

    public Guid? TopCardId { get; set; }

    public Card? TopCard { get; set; }

    public Guid GameId { get; set; }

    public Game Game { get; set; } = null!;

    public ICollection<RoundCard> CardsInPlay { get; set; } = new List<RoundCard>();
}
