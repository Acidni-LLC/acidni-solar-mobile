using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AcidniSolar.Mobile.Services;

namespace AcidniSolar.Mobile.ViewModels;

/// <summary>
/// Settings view model — API key config, refresh interval, preferences.
/// </summary>
public partial class SettingsViewModel : ObservableObject
{
    private readonly ISolarApiService _apiService;
    private readonly ISettingsService _settings;

    public SettingsViewModel(ISolarApiService apiService, ISettingsService settings)
    {
        _apiService = apiService;
        _settings = settings;
    }

    [ObservableProperty]
    private string _apiKey = "";

    [ObservableProperty]
    private bool _isValidating;

    [ObservableProperty]
    private bool _isValid;

    [ObservableProperty]
    private string _validationMessage = "";

    [ObservableProperty]
    private int _refreshInterval;

    [ObservableProperty]
    private bool _notificationsEnabled;

    [ObservableProperty]
    private string _serverVersion = "";

    [ObservableProperty]
    private string _appVersion = "";

    [RelayCommand]
    private async Task LoadSettingsAsync()
    {
        var key = await _settings.GetApiKeyAsync();
        ApiKey = key ?? "";
        RefreshInterval = _settings.RefreshIntervalSeconds;
        NotificationsEnabled = _settings.NotificationsEnabled;
        AppVersion = $"Acidni Solar v{Microsoft.Maui.ApplicationModel.AppInfo.VersionString}";

        // Check server health
        var health = await _apiService.CheckHealthAsync();
        if (health != null)
        {
            ServerVersion = $"Server: {health.Version}";
            IsValid = true;
        }
    }

    [RelayCommand]
    private async Task SaveApiKeyAsync()
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
        {
            ValidationMessage = "API key is required.";
            IsValid = false;
            return;
        }

        IsValidating = true;
        ValidationMessage = "Validating...";

        try
        {
            var valid = await _apiService.ValidateApiKeyAsync(ApiKey.Trim());
            if (valid)
            {
                await _settings.SetApiKeyAsync(ApiKey.Trim());
                IsValid = true;
                ValidationMessage = "API key saved and validated!";
            }
            else
            {
                IsValid = false;
                ValidationMessage = "Invalid API key. Please check and try again.";
            }
        }
        catch (Exception ex)
        {
            IsValid = false;
            ValidationMessage = $"Validation failed: {ex.Message}";
        }
        finally
        {
            IsValidating = false;
        }
    }

    [RelayCommand]
    private void SavePreferences()
    {
        _settings.RefreshIntervalSeconds = RefreshInterval;
        _settings.NotificationsEnabled = NotificationsEnabled;
    }

    [RelayCommand]
    private async Task ClearApiKeyAsync()
    {
        await _settings.RemoveApiKeyAsync();
        ApiKey = "";
        IsValid = false;
        ValidationMessage = "API key removed.";
    }

    [RelayCommand]
    private async Task OpenWebPortalAsync()
    {
        await Browser.OpenAsync("https://solar.acidni.net", BrowserLaunchMode.SystemPreferred);
    }
}
