using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using MouseTracker.App.Models;

namespace MouseTracker.App.Services;

public sealed class HotkeyService : IDisposable
{
    private const int ToggleTrackingHotkeyId = 1;
    private const int ToggleSettingsHotkeyId = 2;

    private readonly Window _messageWindow;
    private HwndSource? _source;
    private IntPtr _handle;
    private HotkeyBinding? _registeredToggleTrackingHotkey;
    private HotkeyBinding? _registeredToggleSettingsHotkey;

    public HotkeyService(Window messageWindow)
    {
        _messageWindow = messageWindow;
    }

    public event EventHandler? ToggleTrackingRequested;

    public event EventHandler? OpenSettingsRequested;

    public bool TryRegister(HotkeyBinding toggleTrackingHotkey, HotkeyBinding toggleSettingsHotkey, out string? error)
    {
        return TryApplyHotkeys(toggleTrackingHotkey, toggleSettingsHotkey, out error);
    }

    public bool TryUpdateHotkeys(HotkeyBinding toggleTrackingHotkey, HotkeyBinding toggleSettingsHotkey, out string? error)
    {
        return TryApplyHotkeys(toggleTrackingHotkey, toggleSettingsHotkey, out error);
    }

    public void Dispose()
    {
        UnregisterCurrentHotkeys();
        _source?.RemoveHook(WndProc);
    }

    private bool TryApplyHotkeys(HotkeyBinding toggleTrackingHotkey, HotkeyBinding toggleSettingsHotkey, out string? error)
    {
        error = null;

        if (!toggleTrackingHotkey.IsValid || !toggleSettingsHotkey.IsValid)
        {
            error = "La hotkey seleccionada no es valida.";
            return false;
        }

        if (toggleTrackingHotkey.Equals(toggleSettingsHotkey))
        {
            error = "Las dos acciones no pueden usar la misma hotkey.";
            return false;
        }

        EnsureMessageSource();

        var previousToggleTrackingHotkey = _registeredToggleTrackingHotkey?.Clone();
        var previousToggleSettingsHotkey = _registeredToggleSettingsHotkey?.Clone();

        UnregisterCurrentHotkeys();

        if (!TryRegisterNativeHotkey(ToggleTrackingHotkeyId, toggleTrackingHotkey, out error))
        {
            TryRestoreHotkeys(previousToggleTrackingHotkey, previousToggleSettingsHotkey);
            return false;
        }

        if (!TryRegisterNativeHotkey(ToggleSettingsHotkeyId, toggleSettingsHotkey, out error))
        {
            NativeMethods.UnregisterHotKey(_handle, ToggleTrackingHotkeyId);
            TryRestoreHotkeys(previousToggleTrackingHotkey, previousToggleSettingsHotkey);
            return false;
        }

        _registeredToggleTrackingHotkey = toggleTrackingHotkey.Clone();
        _registeredToggleSettingsHotkey = toggleSettingsHotkey.Clone();
        return true;
    }

    private void EnsureMessageSource()
    {
        if (_source is not null)
        {
            return;
        }

        _handle = new WindowInteropHelper(_messageWindow).Handle;
        _source = HwndSource.FromHwnd(_handle) ?? throw new InvalidOperationException("Unable to create hotkey message source.");
        _source.AddHook(WndProc);
    }

    private void UnregisterCurrentHotkeys()
    {
        if (_handle == IntPtr.Zero)
        {
            return;
        }

        NativeMethods.UnregisterHotKey(_handle, ToggleTrackingHotkeyId);
        NativeMethods.UnregisterHotKey(_handle, ToggleSettingsHotkeyId);
        _registeredToggleTrackingHotkey = null;
        _registeredToggleSettingsHotkey = null;
    }

    private bool TryRegisterNativeHotkey(int id, HotkeyBinding hotkey, out string? error)
    {
        error = null;
        var modifiers = GetNativeModifiers(hotkey);
        if (NativeMethods.RegisterHotKey(_handle, id, modifiers, (uint)hotkey.VirtualKey))
        {
            return true;
        }

        var exception = new Win32Exception(Marshal.GetLastWin32Error());
        error = $"No se pudo registrar {hotkey.DisplayText}. {exception.Message}";
        return false;
    }

    private void TryRestoreHotkeys(HotkeyBinding? toggleTrackingHotkey, HotkeyBinding? toggleSettingsHotkey)
    {
        if (toggleTrackingHotkey is null || toggleSettingsHotkey is null)
        {
            return;
        }

        if (!TryRegisterNativeHotkey(ToggleTrackingHotkeyId, toggleTrackingHotkey, out _))
        {
            return;
        }

        if (!TryRegisterNativeHotkey(ToggleSettingsHotkeyId, toggleSettingsHotkey, out _))
        {
            NativeMethods.UnregisterHotKey(_handle, ToggleTrackingHotkeyId);
            return;
        }

        _registeredToggleTrackingHotkey = toggleTrackingHotkey.Clone();
        _registeredToggleSettingsHotkey = toggleSettingsHotkey.Clone();
    }

    private static uint GetNativeModifiers(HotkeyBinding hotkey)
    {
        var modifiers = NativeMethods.ModNoRepeat;

        if (hotkey.Control)
        {
            modifiers |= NativeMethods.ModControl;
        }

        if (hotkey.Shift)
        {
            modifiers |= NativeMethods.ModShift;
        }

        if (hotkey.Alt)
        {
            modifiers |= NativeMethods.ModAlt;
        }

        if (hotkey.Windows)
        {
            modifiers |= NativeMethods.ModWin;
        }

        return modifiers;
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg != NativeMethods.WmHotkey)
        {
            return IntPtr.Zero;
        }

        handled = true;

        switch (wParam.ToInt32())
        {
            case ToggleTrackingHotkeyId:
                ToggleTrackingRequested?.Invoke(this, EventArgs.Empty);
                break;
            case ToggleSettingsHotkeyId:
                OpenSettingsRequested?.Invoke(this, EventArgs.Empty);
                break;
        }

        return IntPtr.Zero;
    }
}
