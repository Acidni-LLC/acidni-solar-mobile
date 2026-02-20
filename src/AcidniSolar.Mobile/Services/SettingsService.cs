namespace AcidniSolar.Mobile.Services;

/// <summary>
/// Persistent settings storage for the mobile app.
/// Stores API key and user preferences in SecureStorage / Preferences.
/// </summary>
public interface ISettingsService
{
    Task<string?> GetApiKeyAsync();
    Task SetApiKeyAsync(string apiKey);
    Task RemoveApiKeyAsync();
    bool IsConfigured { get; }
    int RefreshIntervalSeconds { get; set; }
    bool NotificationsEnabled { get; set; }
    string TemperatureUnit { get; set; }
}

public class SettingsService : ISettingsService
{
    private const string ApiKeyStorageKey = "acidni_solar_api_key";
    private const string RefreshIntervalKey = "refresh_interval_seconds";
    private const string NotificationsKey = "notifications_enabled";
    private const string TemperatureUnitKey = "temperature_unit";

    public async Task<string?> GetApiKeyAsync()
    {
        try
        {
            return await SecureStorage.GetAsync(ApiKeyStorageKey);
        }
        catch
        {
            return null;
        }
    }

    public async Task SetApiKeyAsync(string apiKey)
    {
        await SecureStorage.SetAsync(ApiKeyStorageKey, apiKey);
    }

    public async Task RemoveApiKeyAsync()
    {
        SecureStorage.Remove(ApiKeyStorageKey);
        await Task.CompletedTask;
    }

    public bool IsConfigured => SecureStorage.GetAsync(ApiKeyStorageKey).Result != null;

    public int RefreshIntervalSeconds
    {
        get => Preferences.Get(RefreshIntervalKey, 30);
        set => Preferences.Set(RefreshIntervalKey, value);
    }

    public bool NotificationsEnabled
    {
        get => Preferences.Get(NotificationsKey, true);
        set => Preferences.Set(NotificationsKey, value);
    }

    public string TemperatureUnit
    {
        get => Preferences.Get(TemperatureUnitKey, "F");
        set => Preferences.Set(TemperatureUnitKey, value);
    }
}
