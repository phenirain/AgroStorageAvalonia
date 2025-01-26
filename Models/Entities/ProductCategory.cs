using System.Text.Json.Serialization;

namespace desktop.Models.Entities;

public class ProductCategory
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
