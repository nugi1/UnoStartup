namespace UnoServices.Delegates;

public class PlayerAction
{
    public Guid GameId { get; set; } = Guid.NewGuid();
    public string ConnectionId { get; set; } = string.Empty;
    public string ActionType { get; set; } = null!;
    public object? ActionData { get; set; }
}
