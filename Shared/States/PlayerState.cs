using Shared.DTOs;

namespace Shared.States;

public class PlayerState
{
    public string ConnectionId { get; set; } = string.Empty;

    public int Score { get; set; } = 0;

    public IList<CardDto>? Cards { get; set; } = new List<CardDto>();

    public bool UnoSaid { get; set; }

    public string Username { get; set; } = string.Empty;

    public Guid GameId { get; set; }
}
