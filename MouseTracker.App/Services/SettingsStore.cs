using System.IO;
using System.Text.Json;
using System.Windows.Media;
using MouseTracker.App.Models;

namespace MouseTracker.App.Services;

public sealed class SettingsStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public SettingsStore()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        SettingsDirectory = Path.Combine(appData, "mouseTracker");
        SettingsPath = Path.Combine(SettingsDirectory, "settings.json");
    }

    public string SettingsDirectory { get; }

    public string SettingsPath { get; }

    public TrackerSettings Load()
    {
        var settings = TrackerSettings.CreateDefault();
        if (!File.Exists(SettingsPath))
        {
            return settings;
        }

        try
        {
            var json = File.ReadAllText(SettingsPath);
            var dto = JsonSerializer.Deserialize<SettingsDto>(json, JsonOptions);
            if (dto is null)
            {
                return settings;
            }

            if (dto.TrailDurationMilliseconds is not null)
            {
                settings.TrailDurationMilliseconds = dto.TrailDurationMilliseconds.Value;
            }

            if (!string.IsNullOrWhiteSpace(dto.TrailColor) && TryParseColor(dto.TrailColor, out var color))
            {
                settings.TrailColor = color;
            }

            var toggleHotkey = dto.ToggleTrackingHotkey?.IsValid == true
                ? dto.ToggleTrackingHotkey.Clone()
                : settings.ToggleTrackingHotkey;
            var settingsHotkey = dto.ToggleSettingsHotkey?.IsValid == true
                ? dto.ToggleSettingsHotkey.Clone()
                : settings.ToggleSettingsHotkey;

            if (!toggleHotkey.Equals(settingsHotkey))
            {
                settings.ToggleTrackingHotkey = toggleHotkey;
                settings.ToggleSettingsHotkey = settingsHotkey;
            }
        }
        catch
        {
            return TrackerSettings.CreateDefault();
        }

        return settings;
    }

    public void Save(TrackerSettings settings)
    {
        Directory.CreateDirectory(SettingsDirectory);
        var dto = new SettingsDto
        {
            TrailDurationMilliseconds = settings.TrailDurationMilliseconds,
            TrailColor = ToColorString(settings.TrailColor),
            ToggleTrackingHotkey = settings.ToggleTrackingHotkey.Clone(),
            ToggleSettingsHotkey = settings.ToggleSettingsHotkey.Clone()
        };
        var json = JsonSerializer.Serialize(dto, JsonOptions);
        File.WriteAllText(SettingsPath, json);
    }

    private static bool TryParseColor(string value, out Color color)
    {
        try
        {
            var parsed = ColorConverter.ConvertFromString(value);
            if (parsed is Color parsedColor)
            {
                color = parsedColor;
                return true;
            }
        }
        catch
        {
        }

        color = Colors.Cyan;
        return false;
    }

    private static string ToColorString(Color color)
    {
        return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    private sealed class SettingsDto
    {
        public int? TrailDurationMilliseconds { get; set; }

        public string? TrailColor { get; set; }

        public HotkeyBinding? ToggleTrackingHotkey { get; set; }

        public HotkeyBinding? ToggleSettingsHotkey { get; set; }
    }
}
