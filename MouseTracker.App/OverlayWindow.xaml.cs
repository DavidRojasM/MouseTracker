using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using MouseTracker.App.Models;
using MouseTracker.App.Services;

namespace MouseTracker.App;

public partial class OverlayWindow : Window
{
    private readonly DispatcherTimer _renderTimer;

    public OverlayWindow(TrackerSettings settings)
    {
        InitializeComponent();

        Renderer.AttachSettings(settings);
        SetVirtualScreenBounds();

        _renderTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16)
        };
        _renderTimer.Tick += OnRenderTick;
        _renderTimer.Start();

        SourceInitialized += OnSourceInitialized;
    }

    public bool IsTrackingActive { get; set; }

    public void AddClick(Point screenPosition)
    {
        if (!IsTrackingActive)
        {
            return;
        }

        Renderer.AddClickMark(ToOverlayPoint(screenPosition), DateTime.UtcNow);
    }

    private void OnRenderTick(object? sender, EventArgs e)
    {
        var now = DateTime.UtcNow;

        if (IsTrackingActive && NativeMethods.GetCursorPos(out var cursorPosition))
        {
            Renderer.AddTrailPoint(ToOverlayPoint(new Point(cursorPosition.X, cursorPosition.Y)), now);
        }

        Renderer.Tick(now);
    }

    private void OnSourceInitialized(object? sender, EventArgs e)
    {
        var handle = new WindowInteropHelper(this).Handle;
        var extendedStyle = NativeMethods.GetWindowLong(handle, NativeMethods.GwlExStyle);
        extendedStyle |= NativeMethods.WsExTransparent | NativeMethods.WsExToolWindow | NativeMethods.WsExNoActivate;
        NativeMethods.SetWindowLong(handle, NativeMethods.GwlExStyle, extendedStyle);
    }

    private void SetVirtualScreenBounds()
    {
        Left = SystemParameters.VirtualScreenLeft;
        Top = SystemParameters.VirtualScreenTop;
        Width = SystemParameters.VirtualScreenWidth;
        Height = SystemParameters.VirtualScreenHeight;
    }

    private Point ToOverlayPoint(Point screenPoint)
    {
        return new Point(screenPoint.X - Left, screenPoint.Y - Top);
    }
}
