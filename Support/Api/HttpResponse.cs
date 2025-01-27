using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace desktop.Support.Api;

public class HttpResponse<T>
{
    [JsonProperty("message")]
    public string Message { get; set; }
    [JsonProperty("status")]
    public int Status { get; set; }
    [JsonProperty("data")]
    public T? Data { get; set; }
}
