using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using AcidniSolar.Mobile.Services;
using AcidniSolar.Mobile.ViewModels;
using AcidniSolar.Mobile.Views;

namespace AcidniSolar.Mobile;

public static class MauiProgram
{
    /// <summary>
    /// Acidni Solar Mobile — Companion app for real-time solar monitoring.
    /// Connects to the Acidni Solar API at solar.acidni.net.
    /// Version tag format: {YYYYMMDDHHMM}
    /// </summary>
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ── HTTP Client ────────────────────────────────────────────────
        builder.Services.AddHttpClient<ISolarApiService, SolarApiService>(client =>
        {
            client.BaseAddress = new Uri("https://solar.acidni.net");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // ── Services ───────────────────────────────────────────────────
        builder.Services.AddSingleton<ISolarApiService, SolarApiService>();
        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<IConnectivityService, ConnectivityService>();

        // ── ViewModels ─────────────────────────────────────────────────
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<AlertsViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<SystemDetailViewModel>();

        // ── Views ──────────────────────────────────────────────────────
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<AlertsPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<SystemDetailPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
