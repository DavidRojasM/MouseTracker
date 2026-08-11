using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MouseTracker.App.Models;

namespace MouseTracker.App;

public partial class SettingsWindow : Window
{
    private readonly TrackerSettings _settings;
    private bool _allowClose;
    private bool _isInitializing = true;
    private HotkeyCaptureTarget? _captureTarget;

    public SettingsWindow(TrackerSettings settings)
    {
        InitializeComponent();

        _settings = settings;
        ConfigureColorOptions();

        DurationSlider.Value = _settings.TrailDurationMilliseconds;
        SelectCurrentColor();
        UpdateDurationText();
        RefreshHotkeyBindings();
        _settings.PropertyChanged += Settings_PropertyChanged;

        _isInitializing = false;
    }

    public event EventHandler? ToggleTrackingRequested;

    public void SetTrackingState(bool isActive)
    {
        TrackingStateText.Text = isActive ? "Estado: tracking activo" : "Estado: tracking pausado";
        ToggleTrackingButton.Content = isActive ? "Desactivar estela" : "Activar estela";
    }

    public void ForceClose()
    {
        _settings.PropertyChanged -= Settings_PropertyChanged;
        _allowClose = true;
        Close();
    }

    public void RefreshHotkeyBindings()
    {
        ToggleTrackingHotkeyButton.Content = _settings.ToggleTrackingHotkey.DisplayText;
        ToggleSettingsHotkeyButton.Content = _settings.ToggleSettingsHotkey.DisplayText;
        HotkeyHelpText.Text = "Puedes usar combinaciones o teclas sueltas. Las teclas sueltas pueden interferir al escribir o jugar.";
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (!_allowClose && !Application.Current.Dispatcher.HasShutdownStarted)
        {
            _allowClose = true;
            Application.Current.Shutdown();
            return;
        }

        base.OnClosing(e);
    }

    private void ConfigureColorOptions()
    {
        ColorComboBox.Items.Add(new ColorOption("Cian", Colors.Cyan));
        ColorComboBox.Items.Add(new ColorOption("Verde", Colors.LimeGreen));
        ColorComboBox.Items.Add(new ColorOption("Amarillo", Colors.Yellow));
        ColorComboBox.Items.Add(new ColorOption("Rojo", Colors.OrangeRed));
        ColorComboBox.Items.Add(new ColorOption("Violeta", Colors.MediumOrchid));
        ColorComboBox.Items.Add(new ColorOption("Blanco", Colors.White));
    }

    private void SelectCurrentColor()
    {
        foreach (var item in ColorComboBox.Items)
        {
            if (item is ColorOption option && option.Color == _settings.TrailColor)
            {
                ColorComboBox.SelectedItem = option;
                return;
            }
        }

        ColorComboBox.SelectedIndex = 0;
    }

    private void DurationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_isInitializing)
        {
            return;
        }

        _settings.TrailDurationMilliseconds = (int)e.NewValue;
        UpdateDurationText();
    }

    private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isInitializing || ColorComboBox.SelectedItem is not ColorOption option)
        {
            return;
        }

        _settings.TrailColor = option.Color;
    }

    private void ToggleTrackingButton_Click(object sender, RoutedEventArgs e)
    {
        ToggleTrackingRequested?.Invoke(this, EventArgs.Empty);
    }

    private void ToggleTrackingHotkeyButton_Click(object sender, RoutedEventArgs e)
    {
        StartHotkeyCapture(HotkeyCaptureTarget.ToggleTracking);
    }

    private void ToggleSettingsHotkeyButton_Click(object sender, RoutedEventArgs e)
    {
        StartHotkeyCapture(HotkeyCaptureTarget.ToggleSettings);
    }

    private void HotkeyButton_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (_captureTarget is null)
        {
            return;
        }

        e.Handled = true;

        if (e.Key == Key.Escape)
        {
            _captureTarget = null;
            RefreshHotkeyBindings();
            return;
        }

        var hotkey = HotkeyBinding.FromKeyEvent(e);
        if (hotkey is null)
        {
            HotkeyHelpText.Text = "Pulsa una tecla no modificadora. Esc cancela.";
            return;
        }

        if (_captureTarget == HotkeyCaptureTarget.ToggleTracking)
        {
            _settings.ToggleTrackingHotkey = hotkey;
        }
        else
        {
            _settings.ToggleSettingsHotkey = hotkey;
        }

        _captureTarget = null;
        RefreshHotkeyBindings();
    }

    private void HideButton_Click(object sender, RoutedEventArgs e)
    {
        Hide();
    }

    private void UpdateDurationText()
    {
        DurationValueText.Text = $"{_settings.TrailDurationMilliseconds} ms";
    }

    private void StartHotkeyCapture(HotkeyCaptureTarget target)
    {
        _captureTarget = target;

        if (target == HotkeyCaptureTarget.ToggleTracking)
        {
            ToggleTrackingHotkeyButton.Content = "Pulsa teclas...";
            ToggleTrackingHotkeyButton.Focus();
        }
        else
        {
            ToggleSettingsHotkeyButton.Content = "Pulsa teclas...";
            ToggleSettingsHotkeyButton.Focus();
        }

        HotkeyHelpText.Text = "Pulsa la tecla o combinacion nueva. Esc cancela.";
    }

    private void Settings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(TrackerSettings.ToggleTrackingHotkey) or nameof(TrackerSettings.ToggleSettingsHotkey))
        {
            RefreshHotkeyBindings();
        }
    }

    private sealed record ColorOption(string Name, Color Color)
    {
        public override string ToString() => Name;
    }

    private enum HotkeyCaptureTarget
    {
        ToggleTracking,
        ToggleSettings
    }
}
