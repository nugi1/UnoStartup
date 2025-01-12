namespace UnoAccess.Entities;

public class PlayerCard
{
    public Guid PlayerId { get; set; }
    
    public Guid CardId { get; set; }

    public Player Player { get; set; } = null!;

    public Card Card { get; set; } = null!;
}