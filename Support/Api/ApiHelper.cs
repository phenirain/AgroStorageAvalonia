using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using desktop.Exceptions;

namespace desktop.Support.Api;

public static class ApiHelper
{
    private const string _url = "http://localhost:1313/api/";
    
    
    public static async Task<TResponse?> GetAll<TResponse>(string path)
    {
        var client = new HttpClient();
        var response = await client.GetAsync($"{_url}/{path}");
        if (response.StatusCode != HttpStatusCode.OK)
        {
            HttpResponse<TResponse>? result = await response.Content.ReadFromJsonAsync<HttpResponse<TResponse>>();
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
        string json = JsonSerializer.Serialize(entity);
        HttpContent body = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{_url}/{path}", body);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            HttpResponse<TResponse>? result = await response.Content.ReadFromJsonAsync<HttpResponse<TResponse>>();
            if (result != null)
            {
                if (result.Status != 200 || result.Status == 201)
                {
                    throw new RequestException($"Bad request to {_url}/{path}: Message: {result.Message}, Status code: {result.Status}");
                }
                return result.Data!;
            }
            throw new RequestException($"Failed to parse response from {_url}/{path}, Status code: {response.StatusCode}");
        }
        throw new RequestException($"Failed to post to {_url}/{path}: Status code: {response.StatusCode}");
    }

    public static async Task<bool> Put<T>(T entity, string path, int id)
    {
        var client = new HttpClient();
        string json = JsonSerializer.Serialize(entity);
        HttpContent body = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"{_url}/{path}/{id}", body);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            HttpResponse<object>? result = await response.Content.ReadFromJsonAsync<HttpResponse<object>>();
            if (result != null)
            {
                if (result.Status != 200)
                {
                    throw new RequestException($"Bad request to {_url}/{path}/{id}: Message: {result.Message}, Status code: {result.Status}");
                }
                return true;
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
            HttpResponse<object>? result = await response.Content.ReadFromJsonAsync<HttpResponse<object>>();
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
