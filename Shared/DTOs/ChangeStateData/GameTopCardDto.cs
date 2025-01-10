namespace Shared.DTOs.ChangeStateData;

public class GameTopCardDto
{
    public Guid GameId { get;set; }

    public CardDto Card { get; set; } = null!;
}
