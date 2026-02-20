# Acidni Solar on Apple Watch via IFTTT

> Get solar alerts, battery warnings, and grid status on your Apple Watch — no native app required.

## Overview

The Acidni Solar API supports webhooks that can trigger IFTTT applets. These applets push rich notifications to your Apple Watch, iPhone, iPad — any device with the IFTTT app installed.

**What you'll get on your Apple Watch:**
- Battery low alerts (SOC < 20%)
- Solar production milestones (daily goal reached)
- Grid disconnect/reconnect notifications
- Inverter temperature warnings

## Prerequisites

1. **IFTTT Account** — [ifttt.com](https://ifttt.com) (free tier works)
2. **IFTTT App** installed on your iPhone ([App Store](https://apps.apple.com/app/ifttt/id660944635))
3. **Apple Watch** paired with your iPhone
4. **Acidni Solar API key** from the web portal

## Setup Guide

### Step 1: Create a Webhooks Service Connection

1. Open IFTTT → **Explore** → Search "**Webhooks**"
2. Tap **Connect**
3. Go to [ifttt.com/services/maker_webhooks/settings](https://ifttt.com/services/maker_webhooks/settings)
4. Copy your **Webhooks key** (looks like `dX9f3kA2...`)

### Step 2: Create Solar Alert Applets

#### Applet 1: Battery Low Alert

1. IFTTT → **Create** → **If This** → **Webhooks** → "Receive a web request"
2. Event Name: `solar_battery_low`
3. **Then That** → **Notifications** → "Send a rich notification from the IFTTT app"
4. Configure:
   - **Title**: `🔋 Battery Low — {{Value1}}%`
   - **Message**: `Solar battery at {{Value1}}%. Grid status: {{Value2}}. Solar production: {{Value3}}W`
   - **Link URL**: `https://solar.acidni.net`
5. **Finish**

#### Applet 2: Grid Disconnect

1. Create → If This → Webhooks → Event: `solar_grid_disconnect`
2. Then That → Notifications → Rich notification
   - **Title**: `⚡ Grid Disconnected`
   - **Message**: `Running on battery ({{Value1}}%) and solar ({{Value2}}W). Estimated backup: {{Value3}}`
   - **Link URL**: `https://solar.acidni.net`

#### Applet 3: Daily Production Summary

1. Create → If This → Webhooks → Event: `solar_daily_summary`
2. Then That → Notifications → Rich notification
   - **Title**: `☀️ Daily Solar Report`
   - **Message**: `Generated: {{Value1}} kWh | Consumed: {{Value2}} kWh | Self-sufficiency: {{Value3}}%`
   - **Link URL**: `https://solar.acidni.net`

#### Applet 4: Temperature Warning

1. Create → If This → Webhooks → Event: `solar_temp_warning`
2. Then That → Notifications → Rich notification
   - **Title**: `🌡️ Temperature Alert`
   - **Message**: `{{Value1}} at {{Value2}}°F — {{Value3}}`

### Step 3: Configure Webhook Triggers in Solar Portal

The Solar Portal can trigger these webhooks automatically. Configure them in the web portal:

1. Go to [solar.acidni.net](https://solar.acidni.net) → **Settings** → **Integrations**
2. Enable **IFTTT Webhooks**
3. Enter your Webhooks key
4. Select which events to trigger:
   - ✅ Battery below threshold (default: 20%)
   - ✅ Grid disconnect/reconnect
   - ✅ Daily summary (sent at 9 PM)
   - ✅ Temperature warnings

### Step 4: Test on Apple Watch

1. Open IFTTT on iPhone → verify applets are **Connected**
2. On Apple Watch, verify IFTTT notifications are enabled:
   - Watch App → **Notifications** → **IFTTT** → **Allow Notifications**
3. Test: Visit `https://maker.ifttt.com/trigger/solar_battery_low/with/key/YOUR_KEY?value1=15&value2=Connected&value3=2400`
4. You should see the notification appear on your Apple Watch within seconds

## Manual Testing via PowerShell

```powershell
# Replace YOUR_IFTTT_KEY with your actual Webhooks key
$key = "YOUR_IFTTT_KEY"

# Test battery low alert
Invoke-RestMethod -Uri "https://maker.ifttt.com/trigger/solar_battery_low/with/key/$key" `
  -Method POST -ContentType "application/json" `
  -Body '{"value1": "18", "value2": "Connected", "value3": "3200"}'

# Test grid disconnect
Invoke-RestMethod -Uri "https://maker.ifttt.com/trigger/solar_grid_disconnect/with/key/$key" `
  -Method POST -ContentType "application/json" `
  -Body '{"value1": "85", "value2": "4100", "value3": "6 hours"}'

# Test daily summary
Invoke-RestMethod -Uri "https://maker.ifttt.com/trigger/solar_daily_summary/with/key/$key" `
  -Method POST -ContentType "application/json" `
  -Body '{"value1": "42.5", "value2": "31.2", "value3": "87"}'
```

## Apple Watch Complications (Advanced)

For an always-on Apple Watch complication showing battery SOC:

1. Install **Widgetsmith** or **Watchsmith** on iPhone
2. Create a **Shortcut** that calls the Solar API:
   - URL: `https://solar.acidni.net/api/solarapi/widget`
   - Headers: `X-API-Key: YOUR_KEY`
   - Parse JSON → Extract `batterySOC`
3. Display in a Watch complication via Widgetsmith

## iOS Shortcuts Integration

Create an iOS Shortcut for quick solar status:

1. **Shortcuts** app → **+** → **Add Action**
2. **Get Contents of URL**: `https://solar.acidni.net/api/solarapi/widget`
3. Add Header: `X-API-Key` = your key
4. **Get Dictionary Value**: `solarPowerW` from response
5. **Show Result**: `"Solar: [value]W"`
6. Add to Apple Watch as a complication

### Example Shortcut Actions

```
1. URL: https://solar.acidni.net/api/solarapi/widget
2. Get Contents of URL (GET, Header: X-API-Key)
3. Get Dictionary Value "batterySOC" from Contents
4. Set Variable "battery" to Dictionary Value
5. Get Dictionary Value "solarPowerW" from Contents
6. Set Variable "solar" to Dictionary Value
7. Show Alert "☀️ [solar]W | 🔋 [battery]%"
```

## Troubleshooting

| Issue | Solution |
|-------|----------|
| No notification on Watch | Check IFTTT notification permissions in Watch app |
| Delayed notifications | IFTTT free tier may have delays; Pro is near-instant |
| API key rejected | Verify key at solar.acidni.net/api/solarapi/health |
| Webhook not triggering | Check IFTTT activity log at ifttt.com/activity |

## Upgrade Path

When the **Acidni Solar Mobile** native app is available on the App Store, it will provide:
- Native Apple Watch app with complications
- Real-time updates without IFTTT
- Offline data caching
- Push notifications via APNs

Until then, IFTTT + Shortcuts gives you full Apple Watch coverage today.

---

Acidni Solar v20260220-1800  
© 2026 Acidni LLC
