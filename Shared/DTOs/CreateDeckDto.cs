namespace Shared.DTOs;

public class CreateDeckDto
{
    public string OwnerPlayer { get; set; } = null!;

    public int RequestedQuantity { get; set; }
    
    public bool IsPrivate { get; set; }
    
    public string? Password { get; set; }
    
    public int MaxWaitTime { get; set; }
}