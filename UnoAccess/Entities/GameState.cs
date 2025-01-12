using Shared.DTOs;

namespace UnoAccess.Entities;

public class GameState
{
    public Guid? GameId { get; set; }

    public Game? Game { get; set; }

    public bool GameStarted { get; set; }

    public Guid? RoundId { get; set; }

    public IList<PlayerHand> PlayerHands { get; set; } = new List<PlayerHand>();
}
