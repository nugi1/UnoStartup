namespace UnoAccess.Entities;

public class Base
{
    public Base()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
}