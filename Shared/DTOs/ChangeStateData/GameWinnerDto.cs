namespace Shared.DTOs.ChangeStateData;

public class GameWinnerDto
{
    public Guid GameId { get; set; }

    public string ConnectionId { get; set; } = string.Empty;

    public string WinnerName { get; set; } = string.Empty;
}
