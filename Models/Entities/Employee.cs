using Newtonsoft.Json;

namespace desktop.Models.Entities;

public class Employee
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("full_name")]
    public string FullName { get; set; }

    [JsonProperty("login")]
    public string Login { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("role")]
    public Employee Role { get; set; }
}
