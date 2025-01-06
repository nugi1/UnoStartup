namespace UnoAccess.Entities;

public class Base
{
    public Base()
    {
        Id = new Guid();
    }
    public Guid Id { get; set; } = new Guid();
}