namespace UnoAccess.Entities;

public class RoundCard
{
    public int Position { get; set; }

    public Guid RoundId { get; set; }

    public Round Round { get; set; } = null!;

    public Guid CardId { get; set; }

    public Card Card { get; set; } = null!;

    public Guid? PlayerId { get; set; }

    public Player? Player { get; set; }
}
