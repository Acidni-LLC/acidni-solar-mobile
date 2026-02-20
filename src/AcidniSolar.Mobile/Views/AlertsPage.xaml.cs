namespace AcidniSolar.Mobile.Views;

public partial class AlertsPage : ContentPage
{
    public AlertsPage(ViewModels.AlertsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
