namespace UnoServices.Delegates;

public class PlayerAction
{
    public Guid? PlayerId { get; set; }
    public Guid? GameId { get; set; }
    public string Userame { get; set; } = string.Empty;
    public string ActionType { get; set; } = null!;
    public object? ActionData { get; set; }
}
