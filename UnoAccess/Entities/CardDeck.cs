namespace UnoAccess.Entities;

public class CardDeck
{
    public Guid CardId { get; set; }
    
    public Guid DeckId { get; set; }
    
    public int Position { get; set; }
}