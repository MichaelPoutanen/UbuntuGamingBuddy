using System;
using System.Diagnostics;
// ReSharper disable MemberCanBePrivate.Global

namespace UbuntuGamingBuddy.Services;

public static class UbuntuTweaks
{
    public static event Action<bool>? GamingModeChanged;
    public static void DisableShortcuts()
    {
        RunGSettings("set org.gnome.desktop.wm.keybindings switch-applications \"[]\"");
        RunGSettings("set org.gnome.desktop.wm.keybindings switch-windows \"[]\"");
        RunGSettings("set org.gnome.shell.keybindings toggle-overview \"[]\"");
        GamingModeChanged?.Invoke(true);
    }

    public static void EnableShortcuts()
    {
        RunGSettings("reset org.gnome.desktop.wm.keybindings switch-applications");
        RunGSettings("reset org.gnome.desktop.wm.keybindings switch-windows");
        RunGSettings("reset org.gnome.shell.keybindings toggle-overview");
        GamingModeChanged?.Invoke(false);
    }

    public static void DisableHangWarning()
    {
        RunGSettings("set org.gnome.mutter check-alive-timeout 0");
    }

    public static void EnableHangWarning()
    {
        RunGSettings("reset org.gnome.mutter check-alive-timeout");
    }

    public static void EnableFullGamingMode()
    {
        DisableHangWarning();
        DisableShortcuts();
        RunCommand("notify-send 'Ubuntu Gaming Buddy' 'Gaming Mode enabled! Have fun :)'");
    }

    public static void DisableFullGamingMode()
    {
        EnableHangWarning();
        EnableShortcuts();
        RunCommand("notify-send 'Ubuntu Gaming Buddy' 'Gaming Mode disabled! Get back to work!'");

    }
    
    public static bool AreShortcutsDisabled()
    {
        string result = RunCommand("gsettings get org.gnome.desktop.wm.keybindings switch-applications");
        return result.Contains("[]");
    }
    
    private static void RunGSettings(string args)
    {
        RunCommand($"gsettings {args}");
    }
    
    private static string RunCommand(string command)
    {
        using var process = new Process();
        process.StartInfo.FileName = "bash";
        process.StartInfo.Arguments = $"-c \"{command}\"";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.Start();

        string output = process.StandardOutput.ReadToEnd().Trim();
        string error = process.StandardError.ReadToEnd().Trim();
        process.WaitForExit();

        if (!string.IsNullOrEmpty(error))
            Console.WriteLine($"Error in UbuntuTweaks.cs, Error Message:\n{error}");

        return output;
    }
}