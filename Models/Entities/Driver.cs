using Newtonsoft.Json;

namespace desktop.Models.Entities;

public class Driver
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("full_name")]
    public string FullName { get; set; }
}
