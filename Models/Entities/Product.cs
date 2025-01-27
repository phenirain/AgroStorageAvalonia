
using Newtonsoft.Json;

namespace desktop.Models.Entities;

public class Product
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("article")]
    public string Article { get; set; }

    [JsonProperty("category")]
    public ProductCategory Category { get; set; }

    [JsonProperty("quantity_in_stock")]
    public int Quantity { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("location")]
    public string Location { get; set; }

    [JsonProperty("reserved_quantity")]
    public int ReservedQuantity { get; set; }
}
