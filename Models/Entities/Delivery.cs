using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace desktop.Models.Entities;

public enum DeliveryStatus
{
    [EnumMember(Value = "запланирована")]
    Scheduled,
    [EnumMember(Value = "в пути")]
    OnTheWay,
    [EnumMember(Value = "отменен")]
    Canceled,
    [EnumMember(Value = "завершен")]
    Completed
}


public class Delivery
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("order")]
    public Order Order { get; set; }

    [JsonPropertyName("delivery_date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("transport")]
    public string Transport { get; set; }

    [JsonPropertyName("route")]
    public string Route { get; set; }

    [JsonPropertyName("status")]
    public DeliveryStatus Status { get; set; }

    [JsonPropertyName("driver")]
    public Driver Driver { get; set; }
}
