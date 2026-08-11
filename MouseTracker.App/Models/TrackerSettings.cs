using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace MouseTracker.App.Models;

public sealed class TrackerSettings : INotifyPropertyChanged
{
    private int _trailDurationMilliseconds = 700;
    private Color _trailColor = Colors.Cyan;
    private HotkeyBinding _toggleTrackingHotkey = HotkeyBinding.DefaultToggleTracking();
    private HotkeyBinding _toggleSettingsHotkey = HotkeyBinding.DefaultToggleSettings();

    public event PropertyChangedEventHandler? PropertyChanged;

    public int TrailDurationMilliseconds
    {
        get => _trailDurationMilliseconds;
        set
        {
            var clamped = Math.Clamp(value, 100, 3000);
            if (_trailDurationMilliseconds == clamped)
            {
                return;
            }

            _trailDurationMilliseconds = clamped;
            OnPropertyChanged();
        }
    }

    public Color TrailColor
    {
        get => _trailColor;
        set
        {
            if (_trailColor == value)
            {
                return;
            }

            _trailColor = value;
            OnPropertyChanged();
        }
    }

    public HotkeyBinding ToggleTrackingHotkey
    {
        get => _toggleTrackingHotkey;
        set
        {
            var next = value.Clone();
            if (_toggleTrackingHotkey.Equals(next))
            {
                return;
            }

            _toggleTrackingHotkey = next;
            OnPropertyChanged();
        }
    }

    public HotkeyBinding ToggleSettingsHotkey
    {
        get => _toggleSettingsHotkey;
        set
        {
            var next = value.Clone();
            if (_toggleSettingsHotkey.Equals(next))
            {
                return;
            }

            _toggleSettingsHotkey = next;
            OnPropertyChanged();
        }
    }

    public static TrackerSettings CreateDefault()
    {
        return new TrackerSettings();
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
