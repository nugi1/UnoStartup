namespace UnoAccess.Entities;

public class DiscardPileDeck
{
    public Guid CardId { get; set; }
    
    public Guid DeckId { get; set; }
    
    public DateTime Added { get; set; }
}