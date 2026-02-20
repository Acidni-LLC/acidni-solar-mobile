namespace AcidniSolar.Mobile.Views;

public partial class SystemDetailPage : ContentPage
{
    public SystemDetailPage(ViewModels.SystemDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
