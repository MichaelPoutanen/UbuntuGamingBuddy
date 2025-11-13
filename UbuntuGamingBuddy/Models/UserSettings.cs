using System;
using System.ComponentModel.DataAnnotations;

namespace UbuntuGamingBuddy.Models;

public class UserSettings
{
    [Key]
    public int Id { get; set; }

    public string KeyCombination { get; set; } = "Alt+Tab";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}