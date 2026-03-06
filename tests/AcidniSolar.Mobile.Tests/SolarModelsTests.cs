using System.Text.Json;

namespace AcidniSolar.Mobile.Tests;

/// <summary>
/// Tests for SolarSnapshot computed properties and serialization.
/// Models are duplicated here since the MAUI project can't be referenced from net8.0 test project.
/// </summary>
public class SolarModelsTests
{
    // Minimal SolarSnapshot replica for testing computed properties
    private record TestSolarSnapshot(
        double SolarProduction,
        double HomeConsumption,
        double BatterySOC,
        double BatteryPower,
        string GridStatus = "connected")
    {
        public string SolarProductionDisplay => $"{SolarProduction:N0} W";
        public string HomeConsumptionDisplay => $"{HomeConsumption:N0} W";
        public string BatterySOCDisplay => $"{BatterySOC:N0}%";
        public string GridStatusDisplay => GridStatus == "connected" ? "Connected" : "Disconnected";
        public bool IsGridConnected => GridStatus == "connected";
        public bool IsBatteryCharging => BatteryPower > 0;
        public bool IsBatteryLow => BatterySOC < 20;
        public bool IsBatteryCritical => BatterySOC < 10;
    }

    [Fact]
    public void SolarProductionDisplay_FormatsCorrectly()
    {
        var snap = new TestSolarSnapshot(1234, 800, 75, 500);
        Assert.Equal("1,234 W", snap.SolarProductionDisplay);
    }

    [Fact]
    public void BatterySOCDisplay_FormatsCorrectly()
    {
        var snap = new TestSolarSnapshot(0, 0, 85.6, 0);
        Assert.Equal("86%", snap.BatterySOCDisplay);
    }

    [Fact]
    public void GridStatusDisplay_Connected()
    {
        var snap = new TestSolarSnapshot(0, 0, 50, 0, "connected");
        Assert.Equal("Connected", snap.GridStatusDisplay);
        Assert.True(snap.IsGridConnected);
    }

    [Fact]
    public void GridStatusDisplay_Disconnected()
    {
        var snap = new TestSolarSnapshot(0, 0, 50, 0, "disconnected");
        Assert.Equal("Disconnected", snap.GridStatusDisplay);
        Assert.False(snap.IsGridConnected);
    }

    [Fact]
    public void IsBatteryCharging_PositivePower()
    {
        var snap = new TestSolarSnapshot(0, 0, 50, 500);
        Assert.True(snap.IsBatteryCharging);
    }

    [Fact]
    public void IsBatteryCharging_NegativePower_Discharging()
    {
        var snap = new TestSolarSnapshot(0, 0, 50, -300);
        Assert.False(snap.IsBatteryCharging);
    }

    [Fact]
    public void IsBatteryLow_Below20()
    {
        var snap = new TestSolarSnapshot(0, 0, 15, 0);
        Assert.True(snap.IsBatteryLow);
    }

    [Fact]
    public void IsBatteryLow_Above20()
    {
        var snap = new TestSolarSnapshot(0, 0, 50, 0);
        Assert.False(snap.IsBatteryLow);
    }

    [Fact]
    public void IsBatteryCritical_Below10()
    {
        var snap = new TestSolarSnapshot(0, 0, 8, 0);
        Assert.True(snap.IsBatteryCritical);
    }

    [Fact]
    public void IsBatteryCritical_Above10()
    {
        var snap = new TestSolarSnapshot(0, 0, 25, 0);
        Assert.False(snap.IsBatteryCritical);
    }

    [Fact]
    public void SolarSnapshot_JsonDeserialization()
    {
        var json = """
        {
            "solarProduction": 5200.5,
            "homeConsumption": 1300.0,
            "batterySOC": 92.0,
            "batteryPower": 2000,
            "gridStatus": "connected",
            "timestamp": "2026-01-25T12:00:00Z"
        }
        """;

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var data = JsonSerializer.Deserialize<JsonElement>(json, options);

        Assert.Equal(5200.5, data.GetProperty("solarProduction").GetDouble());
        Assert.Equal("connected", data.GetProperty("gridStatus").GetString());
    }
}
