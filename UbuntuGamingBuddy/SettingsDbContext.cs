using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using UbuntuGamingBuddy.Models;

namespace UbuntuGamingBuddy;

public class SettingsDbContext : DbContext
{
    public DbSet<UserSettings> UserSettings { get; set; }

    private static string GetDatabasePath()
    {
        string appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UbuntuGamingBuddy");
        Directory.CreateDirectory(appDir);
        return Path.Combine(appDir, "settings.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={GetDatabasePath()}");
    }
}