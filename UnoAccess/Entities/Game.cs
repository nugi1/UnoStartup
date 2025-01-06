using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;

namespace UnoAccess.Entities;

public class Game : Base
{
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    public DateTime? EndTime { get; set; }

    public Guid DeckId { get; set; }
    public Deck Deck { get; set; } = null!;

    public bool GameOver { get; set; } = false;
    
    public Guid? WinnerId { get; set; }
    public Player? Winner { get; set; } = null!;

    public ICollection<Player> Players = new List<Player>();
}