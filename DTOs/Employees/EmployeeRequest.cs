using System.Text.Json.Serialization;

namespace desktop.DTOs.Employees;

public class CreateEmployeeRequest
{
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("login")]
    public string Login { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("role_id")]
    public int RoleId { get; set; }
}


public class UpdateEmployeeRequest : CreateEmployeeRequest
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
