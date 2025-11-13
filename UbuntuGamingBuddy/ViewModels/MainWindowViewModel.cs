using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UbuntuGamingBuddy.Services;

namespace UbuntuGamingBuddy.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        UpdateShortcutStatus();
    }
    
    [ObservableProperty] private string? _gamingModeStatus;

    private void UpdateShortcutStatus()
    {
        GamingModeStatus = UbuntuTweaks.AreShortcutsDisabled()
            ? "Gaming mode is active"
            : "Gaming mode is not active";
    }
    
    [RelayCommand]
    private void EnableGamingMode()
    {
        UbuntuTweaks.EnableFullGamingMode();
        UpdateShortcutStatus();
    }
    
    [RelayCommand]
    private void DisableGamingMode()
    {
        UbuntuTweaks.DisableFullGamingMode();
        UpdateShortcutStatus();
    }
}