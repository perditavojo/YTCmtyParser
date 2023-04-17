using System.Text.Json.Serialization;

namespace YTApi.Commons.Models;

/// <summary>
/// User 類別
/// </summary>
public class User
{
    [JsonPropertyName("lockedSafetyMode")]
    public bool LockedSafetyMode { get; set; }
}