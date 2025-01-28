
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace desktop.Models.Entities;

public class Client
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("company_name")]
    public string CompanyName { get; set; }

    [JsonProperty("contact_person")]
    public string ContactPerson { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("telephone_number")]
    public string TelephoneNumber { get; set; }
}
