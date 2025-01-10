using Shared.Enums;

namespace UnoAccess.Entities;
public class Card : Base
{
    public CardColor Color { get; set; }
    public CardValue Value { get; set; }
}