using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace desktop.Models.Entities;

public enum DeliveryStatus
{
    [Description("Запланирована")]
    Scheduled,
    [Description("В пути")]
    OnTheWay,
    [Description("Отмененная")]
    Canceled,
    [Description("Завершенная")]
    Completed
}


public class Delivery
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("order")]
    public Order Order { get; set; }

    [JsonProperty("delivery_date")]
    public DateTime Date { get; set; }

    [JsonProperty("transport")]
    public string Transport { get; set; }

    [JsonProperty("route")]
    public string Route { get; set; }

    [JsonProperty("status")]
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public DeliveryStatus Status { get; set; }

    [JsonProperty("driver")]
    public Driver Driver { get; set; }
}
