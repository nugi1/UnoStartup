namespace UnoAccess.Entities;

public class Game : Base
{
    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? WinnerName { get; set; }

    public string? Password { get; set; }

    public Guid? RoundId { get; set; }

    public Round? Round { get; set; }

    public ICollection<Player> Players { get; set; } = new List<Player>();
}