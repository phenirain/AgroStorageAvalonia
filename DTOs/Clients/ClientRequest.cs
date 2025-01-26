using System.Text.Json.Serialization;

namespace desktop.DTOs.Clients;

public class CreateClientRequest
{
    [JsonPropertyName("company_name")]
    public string CompanyName { get; set; }

    [JsonPropertyName("contact_person")]
    public string ContactPerson { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("telephone_number")]
    public string TelephoneNumber { get; set; }
}

public class UpdateClientRequest : CreateClientRequest
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
