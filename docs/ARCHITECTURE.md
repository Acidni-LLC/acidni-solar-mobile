# Solar Mobile — Architecture

> .NET MAUI cross-platform mobile app for monitoring residential solar systems — companion to the Solar Portal.

## System Overview

```text
┌─────────────────────────────────────┐
│  .NET MAUI Mobile App               │
│  ┌──────────┐  ┌──────────────┐     │
│  │  Views    │  │  ViewModels  │     │
│  │  (XAML)   │◀─│  (MVVM)      │     │
│  └──────────┘  └──────┬───────┘     │
│                ┌──────▼───────┐     │
│                │  Services    │     │
│                └──────┬───────┘     │
└───────────────────────┼─────────────┘
                        │
                   Solar Portal API
                   (REST endpoints)
```

## Technology Stack

| Component   | Technology                      |
|-------------|----------------------------------|
| Framework   | .NET MAUI                        |
| Language    | C# 12                            |
| Pattern     | MVVM (ViewModels + Views)        |
| Platforms   | iOS, Android                     |
| API Client  | SolarApiService → Portal REST    |

## Project Structure

| Directory                   | Purpose                          |
|-----------------------------|----------------------------------|
| `src/AcidniSolar.Mobile/`  | Main MAUI project                |
| `Views/`                    | XAML pages (Dashboard, Alerts)   |
| `ViewModels/`               | MVVM view models                 |
| `Models/`                   | Shared data models               |
| `Services/`                 | API client, connectivity, settings |
| `Platforms/Android/`        | Android-specific config          |
| `Platforms/iOS/`            | iOS-specific config              |
| `Resources/`                | Icons, splash, styles            |

## Key Views

| View              | Purpose                              |
|-------------------|--------------------------------------|
| `DashboardPage`   | Main solar system overview           |
| `SystemDetailPage`| Detailed inverter/battery stats      |
| `AlertsPage`      | Active alerts and notifications      |
| `SettingsPage`    | API connection and preference config |

## Related Repos

| Repo                     | Role                       |
|--------------------------|----------------------------|
| `acidni-solar-portal`    | Backend API + web dashboard |
| `acidni-solar-collector` | Background data collection  |

## CMDB Reference

Product code: `solar` (sub-component: mobile) | Status: development | Priority: P2
