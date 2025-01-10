namespace Shared.States;

public class GameStateDto
{
    public Guid GameId { get; set; }

    public int Round { get; set; }

    public int CardsInPlay { get; set; }

    public int PlayersCount { get; set; }

    public List<PlayerState> Players { get; set; } = new List<PlayerState>();
}
