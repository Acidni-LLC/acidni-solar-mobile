using AcidniSolar.Mobile.Views;

namespace AcidniSolar.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for navigation
        Routing.RegisterRoute(nameof(SystemDetailPage), typeof(SystemDetailPage));
    }
}
