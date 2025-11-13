using System;
using System.Linq;
using UbuntuGamingBuddy.Models;

namespace UbuntuGamingBuddy.Services;

public class SettingsService
{
    public UserSettings GetUserSettings()
    {
        try
        {
            using SettingsDbContext db = new();
            db.Database.EnsureCreated();

            // Load or create default settings
            UserSettings? settings = db.UserSettings.FirstOrDefault();

            if (settings == null)
            {
                settings = new UserSettings();
                db.UserSettings.Add(settings);
                db.SaveChanges();
            }

            return settings;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    public void UpdateKeyCombination(string newKeyCombination)
    {
        using SettingsDbContext db = new();
        UserSettings? settings = db.UserSettings.FirstOrDefault();
        if (settings == null)
        {
            settings = new UserSettings { KeyCombination = newKeyCombination };
            db.UserSettings.Add(settings);
        }
        else
        {
            settings.KeyCombination = newKeyCombination;
            db.UserSettings.Update(settings);
        }

        db.SaveChanges();
    }
}