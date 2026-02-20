using CommunityToolkit.Mvvm.ComponentModel;
using AcidniSolar.Mobile.Models;

namespace AcidniSolar.Mobile.ViewModels;

/// <summary>
/// System detail view model (drill-down from dashboard).
/// </summary>
[QueryProperty(nameof(Snapshot), "Snapshot")]
public partial class SystemDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private SolarSnapshot? _snapshot;
}
