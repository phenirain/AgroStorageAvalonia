using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace desktop.Models.Entities;

public enum OrderStatus
{
    Reserved,
    Paid,
    Delivering,
    Completed,
    Canceled
}

public class Order
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("product")]
    public Product Product { get; set; }

    [JsonProperty("client")]
    public Client Client { get; set; }

    [JsonProperty("order_date")]
    public DateTime Date { get; set; }

    [JsonProperty("order_status")]
    [Newtonsoft.Json.JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; set; }

    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("total_price")]
    public decimal TotalPrice { get; set; }
}
