using System.Text.Json.Serialization;
using desktop.DTOs.Orders;

namespace desktop.DTOs.Products;

public class CreateProductRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("article")]
    public string Article { get; set; }

    [JsonPropertyName("category_id")]
    public int CategoryId { get; set; }

    [JsonPropertyName("quantity_in_stock")]
    public int Quantity { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("reserved_quantity")]
    public int ReservedQuantity { get; set; }
}

public class UpdateProductRequest : CreateProductRequest
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
