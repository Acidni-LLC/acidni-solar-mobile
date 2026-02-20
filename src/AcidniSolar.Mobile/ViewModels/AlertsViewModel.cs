using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AcidniSolar.Mobile.Models;
using AcidniSolar.Mobile.Services;

namespace AcidniSolar.Mobile.ViewModels;

/// <summary>
/// Alerts view model — shows recent alert history.
/// </summary>
public partial class AlertsViewModel : ObservableObject
{
    private readonly ISolarApiService _apiService;
    private readonly ISettingsService _settings;

    public AlertsViewModel(ISolarApiService apiService, ISettingsService settings)
    {
        _apiService = apiService;
        _settings = settings;
    }

    [ObservableProperty]
    private List<SolarAlert> _alerts = [];

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private bool _hasAlerts;

    [ObservableProperty]
    private string _errorMessage = "";

    [RelayCommand]
    private async Task LoadAlertsAsync()
    {
        var apiKey = await _settings.GetApiKeyAsync();
        if (string.IsNullOrEmpty(apiKey)) return;

        IsLoading = true;
        ErrorMessage = "";

        try
        {
            Alerts = await _apiService.GetAlertsAsync(apiKey);
            HasAlerts = Alerts.Count > 0;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading alerts: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadAlertsAsync();
    }
}
