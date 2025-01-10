using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

    public Guid? RoundId { get; set; }

    public Guid? Round { get; set; }
        
    [MaxLength(50)]
    public string ConnectionId { get; set; } = null!;
    
    public bool UnoSaid { get; set; }
    
    public DateTime SayUnoUntil { get; set; }
}