
using Newtonsoft.Json;

namespace desktop.Models.Entities;

public class ProductCategory
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}
