namespace Shared.DTOs;

public enum CardValue
{
    Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine,
    DrawTwo, Reverse, Skip, Wild, WildDrawFour
}

public enum CardColor
{
    Red,
    Blue,
    Green,
    Yellow,
    Wild
}

public class CardDto
{
    public CardColor Color { get; set; }
    
    public CardValue Value { get; set; }
}