# Acidni Solar Mobile

> .NET MAUI companion app for the Acidni Solar monitoring platform — iPhone, iPad, Android.

[![Build](https://github.com/Acidni-LLC/acidni-solar-mobile/actions/workflows/build.yml/badge.svg)](https://github.com/Acidni-LLC/acidni-solar-mobile/actions/workflows/build.yml)

## Features

- **Real-time Dashboard** — Solar production, battery SOC, grid status, home consumption at a glance
- **Alert Center** — System alerts with severity levels and timestamps
- **Auto-refresh** — Configurable 10-120 second refresh interval
- **Offline Support** — Graceful degradation when connectivity drops
- **Dark Theme** — Beautiful dark UI with Acidni Solar orange accent
- **Secure API Key** — Stored in platform secure storage (iOS Keychain / Android KeyStore)

## Screenshots

| Dashboard | Alerts | Settings |
|-----------|--------|----------|
| ![Dashboard](docs/screenshots/dashboard.png) | ![Alerts](docs/screenshots/alerts.png) | ![Settings](docs/screenshots/settings.png) |

## Architecture

```
AcidniSolar.Mobile
├── Models/           # Data models (SolarSnapshot, SolarAlert, etc.)
├── Services/         # API client, settings, connectivity
├── ViewModels/       # MVVM ViewModels with CommunityToolkit.Mvvm
├── Views/            # XAML pages (Dashboard, Alerts, Settings, SystemDetail)
├── Converters/       # Value converters for XAML bindings
├── Resources/        # Colors, styles, app icon, splash screen
└── Platforms/        # Android & iOS platform code
```

## Requirements

- .NET 8.0 SDK
- MAUI workload (`dotnet workload install maui`)
- Xcode 15+ (for iOS builds, macOS only)
- Android SDK 24+ (for Android builds)

## Getting Started

### 1. Clone

```bash
git clone https://github.com/Acidni-LLC/acidni-solar-mobile.git
cd acidni-solar-mobile
```

### 2. Restore & Build

```bash
dotnet restore src/AcidniSolar.Mobile/AcidniSolar.Mobile.csproj
dotnet build src/AcidniSolar.Mobile/AcidniSolar.Mobile.csproj -f net8.0-android
```

### 3. Run on Android Emulator

```bash
dotnet build -t:Run -f net8.0-android
```

### 4. Run on iOS Simulator (macOS only)

```bash
dotnet build -t:Run -f net8.0-ios /p:RuntimeIdentifier=iossimulator-arm64
```

## Configuration

The app connects to the Acidni Solar API at `https://solar.acidni.net`. Enter your API key in **Settings** to authenticate.

| Setting | Default | Description |
|---------|---------|-------------|
| API Key | — | Your Solar API key from the web portal |
| Refresh Interval | 30s | Auto-refresh frequency (10-120 seconds) |
| Notifications | On | Push notification alerts |
| Temperature | °F | Fahrenheit or Celsius display |

## API Endpoints

| Endpoint | Description |
|----------|-------------|
| `/api/solarapi/current` | Full real-time solar data |
| `/api/solarapi/widget` | Compact data for widgets |
| `/api/solarapi/daily?days=7` | Daily production/consumption summaries |
| `/api/solarapi/health` | API health check |

## Apple App Store Submission

### Prerequisites

1. Apple Developer account ($99/year) — [developer.apple.com](https://developer.apple.com)
2. App ID registered: `net.acidni.solar`
3. Provisioning profile & signing certificate

### Build for App Store

```bash
dotnet publish src/AcidniSolar.Mobile/AcidniSolar.Mobile.csproj \
  -c Release -f net8.0-ios \
  /p:RuntimeIdentifier=ios-arm64 \
  /p:CodesignKey="Apple Distribution: Acidni LLC" \
  /p:CodesignProvision="Acidni Solar Distribution" \
  /p:ArchiveOnBuild=true
```

### Upload via Transporter

1. Open **Transporter** (macOS App Store)
2. Drag the `.ipa` from `bin/Release/net8.0-ios/ios-arm64/publish/`
3. Submit for review

## Tech Stack

- **.NET 8 MAUI** — Cross-platform framework
- **CommunityToolkit.Mvvm 8.4** — MVVM architecture
- **CommunityToolkit.Maui 9.1** — UI extensions
- **System.Text.Json** — API serialization
- **SecureStorage** — API key encryption

## Related Projects

| Project | Description |
|---------|-------------|
| [acidni-solar-portal](https://github.com/Acidni-LLC/acidni-solar-portal) | Web dashboard (Blazor Server) |
| [terprint-config](https://github.com/Acidni-LLC/terprint-config) | Shared configuration |

---

Acidni Solar v20260220-1800  
© 2026 Acidni LLC
