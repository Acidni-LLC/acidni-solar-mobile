using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AcidniSolar.Mobile.Models;
using AcidniSolar.Mobile.Services;

namespace AcidniSolar.Mobile.ViewModels;

/// <summary>
/// Main dashboard ViewModel — shows real-time solar data.
/// Auto-refreshes at the configured interval.
/// </summary>
public partial class DashboardViewModel : ObservableObject
{
    private readonly ISolarApiService _apiService;
    private readonly ISettingsService _settings;
    private readonly IConnectivityService _connectivity;
    private CancellationTokenSource? _refreshCts;

    public DashboardViewModel(
        ISolarApiService apiService,
        ISettingsService settings,
        IConnectivityService connectivity)
    {
        _apiService = apiService;
        _settings = settings;
        _connectivity = connectivity;
    }

    [ObservableProperty]
    private SolarSnapshot? _currentData;

    [ObservableProperty]
    private SolarWidgetData? _widgetData;

    [ObservableProperty]
    private List<DailySummary> _dailySummaries = [];

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private bool _isConfigured;

    [ObservableProperty]
    private bool _isOffline;

    [ObservableProperty]
    private string _lastUpdated = "";

    [ObservableProperty]
    private string _errorMessage = "";

    // ── Computed display properties ────────────────────────────────
    public string SolarPowerDisplay => CurrentData?.SolarProductionDisplay ?? "-- W";
    public string ConsumptionDisplay => CurrentData?.HomeConsumptionDisplay ?? "-- W";
    public string BatteryDisplay => CurrentData?.BatterySOCDisplay ?? "--%";
    public string GridStatusDisplay => CurrentData?.GridStatusDisplay ?? "--";
    public double BatterySOC => CurrentData?.BatterySOC ?? 0;
    public Color BatteryColor => CurrentData?.BatteryColor ?? Colors.Gray;
    public bool IsGridConnected => CurrentData?.IsGridConnected ?? true;
    public double SelfSufficiency => CurrentData?.SelfSufficiency ?? 0;

    public string TodayProductionDisplay =>
        WidgetData != null ? $"{WidgetData.TodayProduction / 1000.0:N1} kWh" : "-- kWh";
    public string TodayConsumptionDisplay =>
        WidgetData != null ? $"{WidgetData.TodayConsumption / 1000.0:N1} kWh" : "-- kWh";

    // ── Commands ───────────────────────────────────────────────────

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        var apiKey = await _settings.GetApiKeyAsync();
        IsConfigured = !string.IsNullOrEmpty(apiKey);

        if (!IsConfigured)
        {
            ErrorMessage = "Please configure your API key in Settings.";
            return;
        }

        if (!_connectivity.IsConnected)
        {
            IsOffline = true;
            ErrorMessage = "No internet connection.";
            return;
        }

        IsOffline = false;
        IsLoading = true;
        ErrorMessage = "";

        try
        {
            // Fetch current data and widget data in parallel
            var currentTask = _apiService.GetCurrentAsync(apiKey!);
            var widgetTask = _apiService.GetWidgetDataAsync(apiKey!);
            var dailyTask = _apiService.GetDailySummariesAsync(apiKey!, 7);

            await Task.WhenAll(currentTask, widgetTask, dailyTask);

            CurrentData = currentTask.Result;
            WidgetData = widgetTask.Result;
            DailySummaries = dailyTask.Result;

            if (CurrentData != null)
            {
                LastUpdated = $"Updated {DateTime.Now:h:mm tt}";
                OnPropertyChanged(nameof(SolarPowerDisplay));
                OnPropertyChanged(nameof(ConsumptionDisplay));
                OnPropertyChanged(nameof(BatteryDisplay));
                OnPropertyChanged(nameof(GridStatusDisplay));
                OnPropertyChanged(nameof(BatterySOC));
                OnPropertyChanged(nameof(BatteryColor));
                OnPropertyChanged(nameof(IsGridConnected));
                OnPropertyChanged(nameof(SelfSufficiency));
                OnPropertyChanged(nameof(TodayProductionDisplay));
                OnPropertyChanged(nameof(TodayConsumptionDisplay));
            }
            else
            {
                ErrorMessage = "Unable to fetch solar data. Check your API key.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
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
        await LoadDataAsync();
    }

    [RelayCommand]
    private async Task NavigateToSettingsAsync()
    {
        await Shell.Current.GoToAsync("//Settings");
    }

    // ── Auto-refresh lifecycle ─────────────────────────────────────

    public void StartAutoRefresh()
    {
        StopAutoRefresh();
        _refreshCts = new CancellationTokenSource();
        _ = AutoRefreshLoop(_refreshCts.Token);
    }

    public void StopAutoRefresh()
    {
        _refreshCts?.Cancel();
        _refreshCts?.Dispose();
        _refreshCts = null;
    }

    private async Task AutoRefreshLoop(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await LoadDataAsync();
            try
            {
                await Task.Delay(
                    TimeSpan.FromSeconds(_settings.RefreshIntervalSeconds),
                    ct);
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }
    }
}
