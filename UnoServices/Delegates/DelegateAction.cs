using UnoAccess.Entities;

namespace UnoServices.Delegates;

public class DelegateAction
{
    public string ActionName { get; set; } = null!;
    public Func<PlayerAction, Task<GameState>> Action { get; set; } = null!;
}
