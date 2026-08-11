using System.Text.Json.Serialization;
using System.Windows.Input;

namespace MouseTracker.App.Models;

public sealed class HotkeyBinding : IEquatable<HotkeyBinding>
{
    public int VirtualKey { get; set; }

    public bool Control { get; set; }

    public bool Shift { get; set; }

    public bool Alt { get; set; }

    public bool Windows { get; set; }

    [JsonIgnore]
    public bool IsValid => VirtualKey > 0 && KeyInterop.KeyFromVirtualKey(VirtualKey) != Key.None;

    [JsonIgnore]
    public string DisplayText
    {
        get
        {
            var parts = new List<string>();

            if (Control)
            {
                parts.Add("Ctrl");
            }

            if (Shift)
            {
                parts.Add("Shift");
            }

            if (Alt)
            {
                parts.Add("Alt");
            }

            if (Windows)
            {
                parts.Add("Win");
            }

            parts.Add(GetKeyDisplayName(VirtualKey));
            return string.Join(' ', parts);
        }
    }

    public static HotkeyBinding DefaultToggleTracking()
    {
        return new HotkeyBinding
        {
            Control = true,
            VirtualKey = KeyInterop.VirtualKeyFromKey(Key.F9)
        };
    }

    public static HotkeyBinding DefaultToggleSettings()
    {
        return new HotkeyBinding
        {
            Control = true,
            VirtualKey = KeyInterop.VirtualKeyFromKey(Key.F10)
        };
    }

    public static HotkeyBinding? FromKeyEvent(KeyEventArgs e)
    {
        var key = GetActualKey(e);
        if (IsModifierOnlyKey(key))
        {
            return null;
        }

        var virtualKey = KeyInterop.VirtualKeyFromKey(key);
        if (virtualKey <= 0)
        {
            return null;
        }

        var modifiers = Keyboard.Modifiers;
        return new HotkeyBinding
        {
            VirtualKey = virtualKey,
            Control = modifiers.HasFlag(ModifierKeys.Control),
            Shift = modifiers.HasFlag(ModifierKeys.Shift),
            Alt = modifiers.HasFlag(ModifierKeys.Alt),
            Windows = modifiers.HasFlag(ModifierKeys.Windows)
        };
    }

    public HotkeyBinding Clone()
    {
        return new HotkeyBinding
        {
            VirtualKey = VirtualKey,
            Control = Control,
            Shift = Shift,
            Alt = Alt,
            Windows = Windows
        };
    }

    public bool Equals(HotkeyBinding? other)
    {
        return other is not null
            && VirtualKey == other.VirtualKey
            && Control == other.Control
            && Shift == other.Shift
            && Alt == other.Alt
            && Windows == other.Windows;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as HotkeyBinding);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(VirtualKey, Control, Shift, Alt, Windows);
    }

    private static Key GetActualKey(KeyEventArgs e)
    {
        return e.Key switch
        {
            Key.System => e.SystemKey,
            Key.ImeProcessed => e.ImeProcessedKey,
            _ => e.Key
        };
    }

    private static bool IsModifierOnlyKey(Key key)
    {
        return key is Key.LeftCtrl
            or Key.RightCtrl
            or Key.LeftShift
            or Key.RightShift
            or Key.LeftAlt
            or Key.RightAlt
            or Key.LWin
            or Key.RWin
            or Key.Clear;
    }

    private static string GetKeyDisplayName(int virtualKey)
    {
        var key = KeyInterop.KeyFromVirtualKey(virtualKey);
        return key switch
        {
            >= Key.D0 and <= Key.D9 => ((int)key - (int)Key.D0).ToString(),
            >= Key.NumPad0 and <= Key.NumPad9 => $"Num{(int)key - (int)Key.NumPad0}",
            Key.OemPlus => "+",
            Key.OemMinus => "-",
            Key.OemComma => ",",
            Key.OemPeriod => ".",
            Key.OemQuestion => "/",
            Key.OemSemicolon => ";",
            Key.OemQuotes => "'",
            Key.OemOpenBrackets => "[",
            Key.OemCloseBrackets => "]",
            Key.OemPipe => "\\",
            Key.OemTilde => "`",
            _ => key.ToString()
        };
    }
}
