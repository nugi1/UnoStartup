using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;
using UnoAccess.PredefinedData;

namespace UnoAccess.Entities;

public class Deck : Base
{
    [MaxLength(50)]
    public string DeckName { get; init; } = null!;


    public bool IsPrivate { get; init; }
    [MaxLength(50)]
    public string? Password { get; init; }
    
    public int PlayersCount { get; init; }
    
    public DateTime Till { get; init; }
    
    public Guid? TopCardId { get; set; }
    
    public Card? TopCard { get; set; }
    
    [JsonIgnore]
    public Guid? CurrentPlayerId { get; set; }
    
    [JsonIgnore]
    public Player? CurrentPlayer { get; set; }

    [JsonIgnore]
    public IList<Player> Players { get; set; } = new List<Player>();
}
