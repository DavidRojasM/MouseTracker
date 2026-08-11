using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;

namespace MouseTracker.App.Services;

public sealed class MouseHookService : IDisposable
{
    private readonly NativeMethods.LowLevelMouseProc _hookCallback;
    private IntPtr _hookId;

    public MouseHookService()
    {
        _hookCallback = HookCallback;
    }

    public event EventHandler<MouseClickEventArgs>? MouseClick;

    public void Start()
    {
        if (_hookId != IntPtr.Zero)
        {
            return;
        }

        _hookId = NativeMethods.SetWindowsHookEx(
            NativeMethods.WhMouseLl,
            _hookCallback,
            NativeMethods.GetModuleHandle(null),
            0);

        if (_hookId == IntPtr.Zero)
        {
            throw new Win32Exception("Could not install mouse hook.");
        }
    }

    public void Dispose()
    {
        if (_hookId == IntPtr.Zero)
        {
            return;
        }

        NativeMethods.UnhookWindowsHookEx(_hookId);
        _hookId = IntPtr.Zero;
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && IsMouseDownMessage(wParam.ToInt32()))
        {
            var hookStruct = Marshal.PtrToStructure<NativeMethods.MouseHookStruct>(lParam);
            var clickKind = GetClickKind(wParam.ToInt32());
            MouseClick?.Invoke(this, new MouseClickEventArgs(new Point(hookStruct.Point.X, hookStruct.Point.Y), clickKind));
        }

        return NativeMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    private static bool IsMouseDownMessage(int message)
    {
        return message is NativeMethods.WmLButtonDown or NativeMethods.WmRButtonDown or NativeMethods.WmMButtonDown;
    }

    private static MouseClickKind GetClickKind(int message)
    {
        return message switch
        {
            NativeMethods.WmRButtonDown => MouseClickKind.Right,
            NativeMethods.WmMButtonDown => MouseClickKind.Middle,
            _ => MouseClickKind.Left
        };
    }
}

public sealed class MouseClickEventArgs : EventArgs
{
    public MouseClickEventArgs(Point screenPosition, MouseClickKind clickKind)
    {
        ScreenPosition = screenPosition;
        ClickKind = clickKind;
    }

    public Point ScreenPosition { get; }

    public MouseClickKind ClickKind { get; }
}

public enum MouseClickKind
{
    Left,
    Right,
    Middle
}
