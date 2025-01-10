using System.Text.Json.Serialization;

namespace Shared.DTOs.Actions;

public class CreateGameDto
{
    [JsonPropertyName("playerName")]
    public string PlayerName { get; set; } = null!;

    [JsonPropertyName("requestedQuantity")]
    public int RequestedQuantity { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }
}