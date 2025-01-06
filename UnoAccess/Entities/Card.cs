using Shared.DTOs;

namespace UnoAccess.Entities;
public class Card : Base
{
    public CardColor Color { get; set; }
    public CardValue Value { get; set; }
    public bool IsWild => Value == CardValue.Wild || Value == CardValue.WildDrawFour;
}