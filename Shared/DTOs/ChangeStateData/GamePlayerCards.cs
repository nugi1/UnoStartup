using Shared.States;

namespace Shared.DTOs.ChangeStateData;

public class GamePlayerCards
{
    public Guid GameId { get; set; }

    public PlayerState PlayerState { get; set; } = null!;

    public string ConnectionId { get; set; } = string.Empty;

    public IList<CardDto> Cards { get; set; } = new List<CardDto>();
}
