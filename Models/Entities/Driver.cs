using System.Text.Json.Serialization;

namespace desktop.Models.Entities;

public class Driver
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }
}
