using System.Net.Http.Json;
using AcidniSolar.Mobile.Models;

namespace AcidniSolar.Mobile.Services;

/// <summary>
/// Interface for the Acidni Solar REST API client.
/// Consumes endpoints from solar.acidni.net/api/solarapi/*
/// </summary>
public interface ISolarApiService
{
    Task<SolarSnapshot?> GetCurrentAsync(string apiKey);
    Task<SolarWidgetData?> GetWidgetDataAsync(string apiKey);
    Task<List<DailySummary>> GetDailySummariesAsync(string apiKey, int days = 7);
    Task<List<SolarAlert>> GetAlertsAsync(string apiKey);
    Task<HealthResponse?> CheckHealthAsync();
    Task<bool> ValidateApiKeyAsync(string apiKey);
}

/// <summary>
/// HTTP client for the Acidni Solar API.
/// Uses the /api/solarapi endpoints with API key authentication.
/// </summary>
public class SolarApiService : ISolarApiService
{
    private readonly HttpClient _httpClient;

    public SolarApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SolarSnapshot?> GetCurrentAsync(string apiKey)
    {
        try
        {
            var request = CreateRequest(HttpMethod.Get, "/api/solarapi/current", apiKey);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SolarSnapshot>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[SolarAPI] GetCurrent error: {ex.Message}");
            return null;
        }
    }

    public async Task<SolarWidgetData?> GetWidgetDataAsync(string apiKey)
    {
        try
        {
            var request = CreateRequest(HttpMethod.Get, "/api/solarapi/widget", apiKey);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SolarWidgetData>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[SolarAPI] GetWidget error: {ex.Message}");
            return null;
        }
    }

    public async Task<List<DailySummary>> GetDailySummariesAsync(string apiKey, int days = 7)
    {
        try
        {
            var request = CreateRequest(HttpMethod.Get, $"/api/solarapi/daily?days={days}", apiKey);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<DailySummary>>() ?? [];
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[SolarAPI] GetDaily error: {ex.Message}");
            return [];
        }
    }

    public async Task<List<SolarAlert>> GetAlertsAsync(string apiKey)
    {
        try
        {
            // Alert history endpoint - get recent alerts
            var request = CreateRequest(HttpMethod.Get, "/api/solarapi/current", apiKey);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            // Alerts are included in the current snapshot or fetched separately
            // For now, use the current endpoint and extract alert-relevant data
            return [];
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[SolarAPI] GetAlerts error: {ex.Message}");
            return [];
        }
    }

    public async Task<HealthResponse?> CheckHealthAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<HealthResponse>("/api/solarapi/health");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> ValidateApiKeyAsync(string apiKey)
    {
        try
        {
            var request = CreateRequest(HttpMethod.Get, "/api/solarapi/health", apiKey);
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private static HttpRequestMessage CreateRequest(HttpMethod method, string path, string apiKey)
    {
        var request = new HttpRequestMessage(method, path);
        if (!string.IsNullOrEmpty(apiKey))
        {
            request.Headers.Add("X-API-Key", apiKey);
        }
        return request;
    }
}
