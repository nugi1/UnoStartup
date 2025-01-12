namespace Shared.DTOs;

public class PlayerHand
{
    public Guid PlayerId { get; set; }
    
    public IList<CardDto> Cards { get; set; }
}