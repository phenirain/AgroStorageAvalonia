using Newtonsoft.Json;

namespace desktop.Models.Entities;

public class EmployeeRole
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}
