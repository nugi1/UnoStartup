namespace Shared.DTOs.ChangeStateData;

public class GamePlayerCards
{
    public Guid GameId { get; set; }

    public string ConnectionId { get; set; } = string.Empty;

    public IList<CardDto> Cards { get; set; } = new List<CardDto>();

    public string Username { get; set; } = string.Empty;

    public List<string> ConnectionIds { get; set; } = new List<string>();
}
