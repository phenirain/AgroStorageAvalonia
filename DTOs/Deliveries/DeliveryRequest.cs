using System;
using System.Text.Json.Serialization;
using desktop.Models.Entities;

namespace desktop.DTOs.Deliveries;

public class CreateDeliveryRequest
{
    [JsonPropertyName("order_id")]
    
    public int OrderId { get; set; }

    [JsonPropertyName("delivery_date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("transport")]
    public string Transport { get; set; }

    [JsonPropertyName("route")]
    public string Route { get; set; }

    [JsonPropertyName("status")]
    public DeliveryStatus Status { get; set; }

    [JsonPropertyName("driver_id")]
    public int DriverId { get; set; }
}

public class UpdateDeliveryRequest: CreateDeliveryRequest
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
