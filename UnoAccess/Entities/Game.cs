using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;

namespace UnoAccess.Entities;

public class Game : Base
{
    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? WinnerName { get; set; }

    public string? Password { get; set; }

    public ICollection<Player> Players { get; set; } = new Collection<Player>();
}