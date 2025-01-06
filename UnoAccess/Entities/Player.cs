using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UnoAccess.Entities;

public class Player : Base
{
    [MaxLength(50)]
    public string Username { get; set; } = null!;

    public int Score { get; set; }
    
    public bool IsActive { get; set; }
    
    [MaxLength(50)]
    public string ConnectionId { get; set; } = null!;
    
    public Guid GameId { get; set; }
    public Game Game { get; set; }
    
    [JsonIgnore]
    public Guid DeckId { get; set; }
    [JsonIgnore]
    public Deck Deck { get; set; }
    public int Position { get; set; }
    
    public bool UnoSaid { get; set; }
    
    public DateTime SayUnoUntil { get; set; }
}