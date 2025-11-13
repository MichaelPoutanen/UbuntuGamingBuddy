using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using UbuntuGamingBuddy.Services;
using UbuntuGamingBuddy.ViewModels;
using UbuntuGamingBuddy.Views;

namespace UbuntuGamingBuddy;

public partial class App : Application
{
    private TrayIcon? _trayIcon;
    private Window? _mainWindow;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);

            _mainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };

            desktop.MainWindow = _mainWindow;
            InitializeTrayIcon();
            UbuntuTweaks.GamingModeChanged += UpdateTrayIcon;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void InitializeTrayIcon()
    {
        _trayIcon ??= new TrayIcon
        {
            ToolTipText = "Ubuntu Gaming Buddy",
            Icon = new WindowIcon(AssetLoader.Open(new Uri(GetIconPath(!UbuntuTweaks.AreShortcutsDisabled()))))
        };
        UpdateTrayIcon(UbuntuTweaks.AreShortcutsDisabled());


        NativeMenu menu = [];

        NativeMenuItem openItem = new("Open");
        openItem.Click += (s, e) => _mainWindow?.Show();
        menu.Items.Add(openItem);

        menu.Items.Add(new NativeMenuItemSeparator());

        NativeMenuItem toggleItem = new("Toggle Gaming Mode");
        toggleItem.Click += (s, e) => ToggleGamingMode();
        menu.Items.Add(toggleItem);

        menu.Items.Add(new NativeMenuItemSeparator());

        NativeMenuItem quitItem = new("Quit");
        quitItem.Click += (s, e) => Environment.Exit(0);
        menu.Items.Add(quitItem);

        _trayIcon.Menu = menu;
        _trayIcon.IsVisible = true;
    }

    private void ToggleGamingMode()
    {
        bool isDisabled = UbuntuTweaks.AreShortcutsDisabled();
        if (isDisabled)
        {
            UbuntuTweaks.DisableFullGamingMode();
        }
        else
        {
            UbuntuTweaks.EnableFullGamingMode();
        }
    }

    private string GetIconPath(bool gamingModeEnabled)
    {
        return
            gamingModeEnabled
                ? "avares://UbuntuGamingBuddy/Assets/icon_ubuntuGamingBuddy_on.png"
                : "avares://UbuntuGamingBuddy/Assets/icon_ubuntuGamingBuddy_off.png";
    }

    private void UpdateTrayIcon(bool gamingModeEnabled)
    {
        _trayIcon!.Icon = new WindowIcon(AssetLoader.Open(new Uri(GetIconPath(gamingModeEnabled))));
        _trayIcon.ToolTipText = gamingModeEnabled
            ? "Gaming Mode: On"
            : "Gaming Mode: Off";

        _trayIcon.IsVisible = true;
    }
}