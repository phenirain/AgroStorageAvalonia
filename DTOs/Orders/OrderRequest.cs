using System;
using System.Text.Json.Serialization;
using desktop.Models.Entities;

namespace desktop.DTOs.Orders;

public class CreateOrderRequest
{
    [JsonPropertyName("product_id")]
    public int ProductId { get; set; }

    [JsonPropertyName("client_id")]
    public int ClientId { get; set; }

    [JsonPropertyName("order_date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("order_status")]
    public OrderStatus Status { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("total_price")]
    public decimal TotalPrice { get; set; }
}

public class UpdateOrderRequest : CreateOrderRequest
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
