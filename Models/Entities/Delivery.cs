using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace desktop.Models.Entities;

public enum DeliveryStatus
{
    Scheduled,
    OnTheWay,
    Canceled,
    Completed
}


public class Delivery
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("order")]
    public Order Order { get; set; }

    [JsonProperty("date")]
    public DateTime Date { get; set; }

    [JsonProperty("transport")]
    public string Transport { get; set; }

    [JsonProperty("route")]
    public string Route { get; set; }

    [JsonProperty("status")]
    [Newtonsoft.Json.JsonConverter(typeof(JsonStringEnumConverter))]
    public DeliveryStatus Status { get; set; }

    [JsonProperty("driver")]
    public Driver Driver { get; set; }
}
