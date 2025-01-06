namespace UnoAccess.Entities;

public class GameLog : Base
{
    public Guid GameId { get; set; }
    
    public Guid PlayerId { get; set; }

    public string Action { get; set; } = null!;
    
    public DateTime PlayedAt { get; set; }
}