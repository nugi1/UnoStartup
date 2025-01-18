namespace Shared.DTOs.ChangeStateData;

public class GameScoreDto
{
    public string ConnectionId { get; set; } = string.Empty;

    public int Score { get; set; }

    public string Username { get; set; } = string.Empty;
}
