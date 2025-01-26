using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace desktop.Models.Entities;

public enum OrderStatus
{
    [EnumMember(Value = "зарезервирован")]
    Reserved,
    [EnumMember(Value = "оплачен")]
    Paid,
    [EnumMember(Value = "в доставке")]
    Delivering,
    [EnumMember(Value = "завершен")]
    Completed,
    [EnumMember(Value = "отменен")]
    Canceled
}

public class Order
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("product")]
    public Product Product { get; set; }

    [JsonPropertyName("client")]
    public Client Client { get; set; }

    [JsonPropertyName("order_date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("order_status")]
    public OrderStatus Status { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("total_price")]
    public decimal TotalPrice { get; set; }
}
