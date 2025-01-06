namespace Shared.DTOs;

public class CreateGameDto
{
    public string PlayerName { get; set; } = null!;

    public int RequestedQuantity { get; set; }
    
    public bool IsPrivate { get; set; }
    
    public string? Password { get; set; }
    
    public int MaxWaitTime { get; set; }
}