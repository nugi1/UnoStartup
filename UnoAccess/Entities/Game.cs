namespace UnoAccess.Entities;

public class Game : Base
{
    public Game()
    {
        StartTime = DateTime.Now.ToUniversalTime();
    }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int PlayerQuantity { get; set; }

    public string? WinnerName { get; set; }

    public string? Password { get; set; }

    public Guid? RoundId { get; set; }

    public Round? Round { get; set; }

    public IList<Player> Players { get; set; } = new List<Player>();
}