namespace Shared.DTOs.ChangeStateData;

public class GameTurnDto
{
    public string ConnectionId { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public Guid GameId { get; set; }
}
