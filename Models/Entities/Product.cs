using System.Text.Json.Serialization;

namespace desktop.Models.Entities;

public class Product
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("article")]
    public string Article { get; set; }

    [JsonPropertyName("category")]
    public ProductCategory Category { get; set; }

    [JsonPropertyName("quantity_in_stock")]
    public int Quantity { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("reserved_quantity")]
    public int ReservedQuantity { get; set; }
}
