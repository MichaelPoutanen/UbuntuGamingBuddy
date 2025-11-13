using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UbuntuGamingBuddy.Models;
using UbuntuGamingBuddy.Services;

namespace UbuntuGamingBuddy.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        IsGamingModeEnabled = UbuntuTweaks.AreShortcutsDisabled();
        UbuntuTweaks.GamingModeChanged += OnGamingModeChanged;
    }

    [ObservableProperty] private bool _isGamingModeEnabled;
    
    public string GamingModeStatus => IsGamingModeEnabled? "Gaming mode is active"
        : "Gaming mode is not active";
    
    
    [RelayCommand]
    private void EnableGamingMode()
    {
        UbuntuTweaks.EnableFullGamingMode();
    }
    
    [RelayCommand]
    private void DisableGamingMode()
    {
        UbuntuTweaks.DisableFullGamingMode();
    }
    
    private void OnGamingModeChanged(bool enabled)
    {
        IsGamingModeEnabled = enabled;
        OnPropertyChanged(nameof(GamingModeStatus));
    }
}