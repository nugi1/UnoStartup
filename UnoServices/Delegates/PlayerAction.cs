namespace UnoServices.Delegates;

public class PlayerAction
{
    public Guid? GameId { get; set; }
    public string ConnectionId { get; set; } = string.Empty;
    public string ActionType { get; set; } = null!;
    public object? ActionData { get; set; }
}
