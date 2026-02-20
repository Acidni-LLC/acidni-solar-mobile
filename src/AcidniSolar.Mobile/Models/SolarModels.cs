using System.Text.Json.Serialization;

namespace AcidniSolar.Mobile.Models;

/// <summary>
/// Real-time solar system snapshot from /api/solarapi/current
/// </summary>
public class SolarSnapshot
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("solarProduction")]
    public double SolarProduction { get; set; }

    [JsonPropertyName("homeConsumption")]
    public double HomeConsumption { get; set; }

    [JsonPropertyName("gridImport")]
    public double GridImport { get; set; }

    [JsonPropertyName("gridExport")]
    public double GridExport { get; set; }

    [JsonPropertyName("batterySOC")]
    public double BatterySOC { get; set; }

    [JsonPropertyName("batteryPower")]
    public double BatteryPower { get; set; }

    [JsonPropertyName("batteryVoltage")]
    public double BatteryVoltage { get; set; }

    [JsonPropertyName("inverterTemperature")]
    public double InverterTemperature { get; set; }

    [JsonPropertyName("gridStatus")]
    public string GridStatus { get; set; } = "connected";

    [JsonPropertyName("generatorStatus")]
    public string? GeneratorStatus { get; set; }

    [JsonPropertyName("inverterStatus")]
    public string InverterStatus { get; set; } = "online";

    [JsonPropertyName("selfSufficiency")]
    public double SelfSufficiency { get; set; }

    // Computed display properties
    public string SolarProductionDisplay => $"{SolarProduction:N0} W";
    public string HomeConsumptionDisplay => $"{HomeConsumption:N0} W";
    public string BatterySOCDisplay => $"{BatterySOC:N0}%";
    public string GridStatusDisplay => GridStatus == "connected" ? "Connected" : "Disconnected";
    public bool IsGridConnected => GridStatus == "connected";
    public bool IsBatteryCharging => BatteryPower > 0;
    public bool IsBatteryLow => BatterySOC < 20;
    public bool IsBatteryCritical => BatterySOC < 10;

    public Color BatteryColor => BatterySOC switch
    {
        < 10 => Colors.Red,
        < 20 => Colors.OrangeRed,
        < 50 => Colors.Orange,
        _ => Colors.LimeGreen
    };
}

/// <summary>
/// Compact widget data from /api/solarapi/widget (designed for smartwatch/widgets)
/// </summary>
public class SolarWidgetData
{
    [JsonPropertyName("production")]
    public double Production { get; set; }

    [JsonPropertyName("consumption")]
    public double Consumption { get; set; }

    [JsonPropertyName("battery")]
    public double Battery { get; set; }

    [JsonPropertyName("grid")]
    public string Grid { get; set; } = "connected";

    [JsonPropertyName("selfSufficiency")]
    public double SelfSufficiency { get; set; }

    [JsonPropertyName("todayProduction")]
    public double TodayProduction { get; set; }

    [JsonPropertyName("todayConsumption")]
    public double TodayConsumption { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Daily summary from /api/solarapi/daily
/// </summary>
public class DailySummary
{
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("totalProduction")]
    public double TotalProduction { get; set; }

    [JsonPropertyName("totalConsumption")]
    public double TotalConsumption { get; set; }

    [JsonPropertyName("totalGridImport")]
    public double TotalGridImport { get; set; }

    [JsonPropertyName("totalGridExport")]
    public double TotalGridExport { get; set; }

    [JsonPropertyName("peakProduction")]
    public double PeakProduction { get; set; }

    [JsonPropertyName("avgBatterySOC")]
    public double AvgBatterySOC { get; set; }

    [JsonPropertyName("selfSufficiency")]
    public double SelfSufficiency { get; set; }

    // Display
    public string ProductionDisplay => $"{TotalProduction / 1000.0:N1} kWh";
    public string ConsumptionDisplay => $"{TotalConsumption / 1000.0:N1} kWh";
    public string DateDisplay => Date.ToString("MMM dd");
}

/// <summary>
/// Alert record from the alerts system
/// </summary>
public class SolarAlert
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("alertType")]
    public string AlertType { get; set; } = "";

    [JsonPropertyName("severity")]
    public string Severity { get; set; } = "info";

    [JsonPropertyName("message")]
    public string Message { get; set; } = "";

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("acknowledged")]
    public bool Acknowledged { get; set; }

    [JsonPropertyName("value")]
    public double? Value { get; set; }

    [JsonPropertyName("threshold")]
    public double? Threshold { get; set; }

    // Display
    public string TimeAgo
    {
        get
        {
            var span = DateTime.UtcNow - Timestamp;
            if (span.TotalMinutes < 1) return "Just now";
            if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes}m ago";
            if (span.TotalHours < 24) return $"{(int)span.TotalHours}h ago";
            return $"{(int)span.TotalDays}d ago";
        }
    }

    public Color SeverityColor => Severity.ToLowerInvariant() switch
    {
        "critical" => Colors.Red,
        "warning" => Colors.Orange,
        "info" => Colors.DodgerBlue,
        _ => Colors.Gray
    };

    public string SeverityIcon => Severity.ToLowerInvariant() switch
    {
        "critical" => "🔴",
        "warning" => "🟡",
        "info" => "🔵",
        _ => "⚪"
    };
}

/// <summary>
/// API health check response
/// </summary>
public class HealthResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = "";

    [JsonPropertyName("version")]
    public string Version { get; set; } = "";
}
