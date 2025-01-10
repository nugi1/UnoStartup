using Shared.States;

namespace Shared.DTOs.ChangeStateData;

public class GameCreatedDto
{
    public Guid GameId { get; set; }

    public List<PlayerState> Players { get; set; } = new List<PlayerState>();
}
