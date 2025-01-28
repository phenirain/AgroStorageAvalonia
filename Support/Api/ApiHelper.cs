using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using desktop.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace desktop.Support.Api;

public static class ApiHelper
{
    private const string _url = "http://82.97.241.71:1313/api";
    
    private static JsonSerializerSettings settings = new JsonSerializerSettings
    {
        Converters = { new StringEnumConverter() }, 
        Formatting = Formatting.Indented,
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };
    
    public static async Task<TResponse?> GetAll<TResponse>(string path)
    {
        var client = new HttpClient();
        var response = await client.GetAsync($"{_url}/{path}");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            string responseText = await response.Content.ReadAsStringAsync();
            HttpResponse<TResponse>? result = JsonConvert.DeserializeObject<HttpResponse<TResponse>?>(responseText, settings: settings);
            if (result != null)
            {
                if (result.Status != 200)
                {
                    throw new RequestException($"Bad request to {_url}/{path}: Message: {result.Message}, Status code: {result.Status}");
                }

                return result.Data;
            }
            throw new RequestException($"Failed to parse response from {_url}/{path}: Status code: {response.StatusCode}");
        }
        throw new RequestException($"Failed to get all from {_url}/{path}: Status code: {response.StatusCode}");
    }

    public static async Task<TResponse> Post<TResponse, T>(T entity, string path)
    {
        var client = new HttpClient();
        string json = JsonConvert.SerializeObject(entity, settings);
        HttpContent body = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{_url}/{path}", body);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            HttpResponse<TResponse>? result = JsonConvert.DeserializeObject<HttpResponse<TResponse>>(await response.Content.ReadAsStringAsync(), settings);
            if (result != null)
            {
                if (result.Status == 200 || result.Status == 201)
                {
                    return result.Data!;
                }
                throw new RequestException($"Bad request to {_url}/{path}: Message: {result.Message}, Status code: {result.Status}");
            }
            throw new RequestException($"Failed to parse response from {_url}/{path}, Status code: {response.StatusCode}");
        }
        throw new RequestException($"Failed to post to {_url}/{path}: Status code: {response.StatusCode}");
    }

    public static async Task Put<T>(T entity, string path, int id)
    {
        var client = new HttpClient();
        string json = JsonConvert.SerializeObject(entity, settings);
        HttpContent body = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"{_url}/{path}/{id}", body);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            HttpResponse<object>? result = JsonConvert.DeserializeObject<HttpResponse<object>>(await response.Content.ReadAsStringAsync(), settings);
            if (result != null)
            {
                if (result.Status != 200)
                {
                    throw new RequestException($"Bad request to {_url}/{path}/{id}: Message: {result.Message}, Status code: {result.Status}");
                }
                return;
            }
        }
        throw new RequestException($"Failed to put to {_url}/{path}/{id}: Status code: {response.StatusCode}");
    }

    public static async Task<bool> Delete(string path, int id)
    {
        var client = new HttpClient();
        var response = await client.DeleteAsync($"{_url}/{path}/{id}");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            HttpResponse<object>? result =
                JsonConvert.DeserializeObject<HttpResponse<object>>(await response.Content.ReadAsStringAsync(), settings);
            if (result != null)
            {
                if (result.Status!= 200)
                {
                    return false;
                }
                return true;
            }
            throw new RequestException($"Failed to delete from {_url}/{path}/{id}");
        }
        throw new RequestException($"Failed to delete from {_url}/{path}/{id}: Status code: {response.StatusCode}");
    }
}
