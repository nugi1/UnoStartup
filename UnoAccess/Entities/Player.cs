using System.ComponentModel.DataAnnotations;

namespace UnoAccess.Entities;

public class Player : Base
{
    [MaxLength(50)]
    public string Username { get; set; } = null!;

    public int Score { get; set; }

    public bool IsWinner { get; set; }

    public bool IsTurn { get; set; }

    public int Position { get; set; }

    public bool IsOwner { get; set; }
    
    public Guid GameId { get; set; }
    public Game Game { get; set; }
        
    [MaxLength(50)]
    public string ConnectionId { get; set; } = null!;
    
    public bool UnoSaid { get; set; }
    
    public DateTime SayUnoUntil { get; set; }

    public int DrawStack { get; set; }
}