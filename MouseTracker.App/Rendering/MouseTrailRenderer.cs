using System.Windows;
using System.Windows.Media;
using MouseTracker.App.Models;

namespace MouseTracker.App.Rendering;

public sealed class MouseTrailRenderer : FrameworkElement
{
    private const int ClickMarkDurationMilliseconds = 450;
    private readonly List<TrailPoint> _trailPoints = [];
    private readonly List<ClickMark> _clickMarks = [];
    private TrackerSettings? _settings;

    public void AttachSettings(TrackerSettings settings)
    {
        if (_settings is not null)
        {
            _settings.PropertyChanged -= OnSettingsChanged;
        }

        _settings = settings;
        _settings.PropertyChanged += OnSettingsChanged;
        InvalidateVisual();
    }

    public void AddTrailPoint(Point position, DateTime timestamp)
    {
        if (_trailPoints.Count > 0)
        {
            var previous = _trailPoints[^1].Position;
            if ((previous - position).LengthSquared < 1)
            {
                return;
            }
        }

        _trailPoints.Add(new TrailPoint(position, timestamp));
    }

    public void AddClickMark(Point position, DateTime timestamp)
    {
        _clickMarks.Add(new ClickMark(position, timestamp));
    }

    public void Tick(DateTime now)
    {
        if (_settings is null)
        {
            return;
        }

        Prune(now);
        InvalidateVisual();
    }

    public void Clear()
    {
        _trailPoints.Clear();
        _clickMarks.Clear();
        InvalidateVisual();
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        if (_settings is null)
        {
            return;
        }

        var now = DateTime.UtcNow;

        DrawTrail(drawingContext, now, _settings);
        DrawClickMarks(drawingContext, now, _settings);
    }

    private void DrawTrail(DrawingContext drawingContext, DateTime now, TrackerSettings settings)
    {
        if (_trailPoints.Count < 2)
        {
            return;
        }

        for (var i = 1; i < _trailPoints.Count; i++)
        {
            var previous = _trailPoints[i - 1];
            var current = _trailPoints[i];
            var opacity = GetOpacity(now, current.Timestamp, settings.TrailDurationMilliseconds);

            if (opacity <= 0)
            {
                continue;
            }

            var color = settings.TrailColor;
            color.A = (byte)(220 * opacity);

            var brush = new SolidColorBrush(color);
            brush.Freeze();

            var thickness = Math.Max(2, 7 * opacity);
            var pen = new Pen(brush, thickness)
            {
                StartLineCap = PenLineCap.Round,
                EndLineCap = PenLineCap.Round
            };
            pen.Freeze();

            drawingContext.DrawLine(pen, previous.Position, current.Position);
        }
    }

    private void DrawClickMarks(DrawingContext drawingContext, DateTime now, TrackerSettings settings)
    {
        foreach (var mark in _clickMarks)
        {
            var elapsed = (now - mark.Timestamp).TotalMilliseconds;
            var progress = Math.Clamp(elapsed / ClickMarkDurationMilliseconds, 0, 1);
            var opacity = 1 - progress;

            if (opacity <= 0)
            {
                continue;
            }

            var color = settings.TrailColor;
            color.A = (byte)(240 * opacity);

            var brush = new SolidColorBrush(color);
            brush.Freeze();

            var radius = 8 + 18 * progress;
            var pen = new Pen(brush, 2.5);
            pen.Freeze();

            drawingContext.DrawEllipse(null, pen, mark.Position, radius, radius);
        }
    }

    private void Prune(DateTime now)
    {
        if (_settings is null)
        {
            return;
        }

        _trailPoints.RemoveAll(point => (now - point.Timestamp).TotalMilliseconds > _settings.TrailDurationMilliseconds);
        _clickMarks.RemoveAll(mark => (now - mark.Timestamp).TotalMilliseconds > ClickMarkDurationMilliseconds);
    }

    private static double GetOpacity(DateTime now, DateTime timestamp, int durationMilliseconds)
    {
        var age = (now - timestamp).TotalMilliseconds;
        return Math.Clamp(1 - age / durationMilliseconds, 0, 1);
    }

    private void OnSettingsChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        InvalidateVisual();
    }

    private readonly record struct TrailPoint(Point Position, DateTime Timestamp);

    private readonly record struct ClickMark(Point Position, DateTime Timestamp);
}
