using System.Text.Json.Serialization;

namespace Shared.DTOs.Actions;

public class JoinGameDto
{
    [JsonPropertyName("gameId")]
    public Guid GameId { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
}
