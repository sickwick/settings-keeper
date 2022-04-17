using System.Text.Json.Serialization;

namespace SettingsKeeper.Api.Models;

public class ErrorViewModel
{
    [JsonPropertyName("code")]
    public int StatusCode { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}