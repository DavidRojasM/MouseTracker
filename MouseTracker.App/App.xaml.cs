using System.Windows;
using System.ComponentModel;
using MouseTracker.App.Models;
using MouseTracker.App.Services;

namespace MouseTracker.App;

public partial class App : Application
{
    private TrackerSettings? _settings;
    private OverlayWindow? _overlayWindow;
    private SettingsWindow? _settingsWindow;
    private HotkeyService? _hotkeyService;
    private MouseHookService? _mouseHookService;
    private SettingsStore? _settingsStore;
    private HotkeyBinding? _lastRegisteredToggleTrackingHotkey;
    private HotkeyBinding? _lastRegisteredToggleSettingsHotkey;
    private bool _isRollingBackHotkeys;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        _settingsStore = new SettingsStore();
        _settings = _settingsStore.Load();
        _settings.PropertyChanged += OnSettingsChanged;

        _overlayWindow = new OverlayWindow(_settings);
        _overlayWindow.Show();

        _hotkeyService = new HotkeyService(_overlayWindow);
        _hotkeyService.ToggleTrackingRequested += (_, _) => ToggleTracking();
        _hotkeyService.OpenSettingsRequested += (_, _) => ToggleSettingsWindow();
        if (_hotkeyService.TryRegister(_settings.ToggleTrackingHotkey, _settings.ToggleSettingsHotkey, out var hotkeyError))
        {
            StoreRegisteredHotkeys();
        }
        else
        {
            MessageBox.Show(
                hotkeyError ?? "No se pudieron registrar las hotkeys.",
                "mouseTracker",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        _mouseHookService = new MouseHookService();
        _mouseHookService.MouseClick += OnMouseClick;
        _mouseHookService.Start();

        ShowSettingsWindow();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _mouseHookService?.Dispose();
        _hotkeyService?.Dispose();
        if (_settings is not null)
        {
            _settings.PropertyChanged -= OnSettingsChanged;
        }

        _settingsWindow?.ForceClose();
        _overlayWindow?.Close();

        base.OnExit(e);
    }

    private void ToggleTracking()
    {
        if (_overlayWindow is null)
        {
            return;
        }

        _overlayWindow.IsTrackingActive = !_overlayWindow.IsTrackingActive;
        _settingsWindow?.SetTrackingState(_overlayWindow.IsTrackingActive);
    }

    private void ShowSettingsWindow()
    {
        if (_settings is null || _overlayWindow is null)
        {
            return;
        }

        if (_settingsWindow is null)
        {
            _settingsWindow = new SettingsWindow(_settings);
            _settingsWindow.ToggleTrackingRequested += (_, _) => ToggleTracking();
        }

        _settingsWindow.SetTrackingState(_overlayWindow.IsTrackingActive);

        if (!_settingsWindow.IsVisible)
        {
            _settingsWindow.Show();
        }

        if (_settingsWindow.WindowState == WindowState.Minimized)
        {
            _settingsWindow.WindowState = WindowState.Normal;
        }

        _settingsWindow.Activate();
    }

    private void ToggleSettingsWindow()
    {
        if (_settingsWindow is { IsVisible: true, WindowState: not WindowState.Minimized })
        {
            _settingsWindow.Hide();
            return;
        }

        ShowSettingsWindow();
    }

    private void OnMouseClick(object? sender, MouseClickEventArgs e)
    {
        if (_overlayWindow is null)
        {
            return;
        }

        Dispatcher.BeginInvoke(() => _overlayWindow.AddClick(e.ScreenPosition));
    }

    private void OnSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_settings is null || _settingsStore is null)
        {
            return;
        }

        if (_isRollingBackHotkeys)
        {
            return;
        }

        if (e.PropertyName is nameof(TrackerSettings.ToggleTrackingHotkey) or nameof(TrackerSettings.ToggleSettingsHotkey))
        {
            ApplyHotkeyChangeOrRollback();
            return;
        }

        _settingsStore.Save(_settings);
    }

    private void ApplyHotkeyChangeOrRollback()
    {
        if (_settings is null || _settingsStore is null || _hotkeyService is null)
        {
            return;
        }

        if (_hotkeyService.TryUpdateHotkeys(_settings.ToggleTrackingHotkey, _settings.ToggleSettingsHotkey, out var error))
        {
            StoreRegisteredHotkeys();
            _settingsStore.Save(_settings);
            _settingsWindow?.RefreshHotkeyBindings();
            return;
        }

        MessageBox.Show(
            error ?? "No se pudo aplicar la hotkey seleccionada.",
            "mouseTracker",
            MessageBoxButton.OK,
            MessageBoxImage.Warning);

        if (_lastRegisteredToggleTrackingHotkey is not null && _lastRegisteredToggleSettingsHotkey is not null)
        {
            _isRollingBackHotkeys = true;
            _settings.ToggleTrackingHotkey = _lastRegisteredToggleTrackingHotkey.Clone();
            _settings.ToggleSettingsHotkey = _lastRegisteredToggleSettingsHotkey.Clone();
            _isRollingBackHotkeys = false;
            _settingsStore.Save(_settings);
            _settingsWindow?.RefreshHotkeyBindings();
        }
    }

    private void StoreRegisteredHotkeys()
    {
        if (_settings is null)
        {
            return;
        }

        _lastRegisteredToggleTrackingHotkey = _settings.ToggleTrackingHotkey.Clone();
        _lastRegisteredToggleSettingsHotkey = _settings.ToggleSettingsHotkey.Clone();
    }
}
