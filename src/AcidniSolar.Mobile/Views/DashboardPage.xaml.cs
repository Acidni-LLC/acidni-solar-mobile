namespace AcidniSolar.Mobile.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(ViewModels.DashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
