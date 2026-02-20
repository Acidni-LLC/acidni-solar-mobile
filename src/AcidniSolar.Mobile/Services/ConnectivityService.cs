namespace AcidniSolar.Mobile.Services;

/// <summary>
/// Wraps MAUI Connectivity for DI/testability.
/// </summary>
public interface IConnectivityService
{
    bool IsConnected { get; }
    event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;
}

public class ConnectivityService : IConnectivityService
{
    public ConnectivityService()
    {
        Connectivity.ConnectivityChanged += OnConnectivityChanged;
    }

    public bool IsConnected =>
        Connectivity.NetworkAccess == NetworkAccess.Internet;

    public event EventHandler<ConnectivityChangedEventArgs>? ConnectivityChanged;

    private void OnConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
    {
        ConnectivityChanged?.Invoke(this, e);
    }
}
