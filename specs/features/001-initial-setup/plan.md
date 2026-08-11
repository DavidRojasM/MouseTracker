# Plan: Initial Setup

## Enfoque tecnico

Usar .NET 8, C# y WPF para construir una aplicacion local Windows. El overlay sera una ventana transparente, topmost y click-through. La posicion del raton se obtendra mediante Win32 y el dibujo se actualizara en un `DispatcherTimer`.

## Componentes

- `App`: ciclo de vida, creacion de servicios y cierre limpio.
- `OverlayWindow`: ventana transparente que dibuja la estela y marcas.
- `SettingsWindow`: UI basica para cambiar configuracion.
- `HotkeyService`: registro de `Ctrl F9` y `Ctrl F10` con `RegisterHotKey` usando Control como modificador. No se registra la tecla `+`.
- `MouseHookService`: hook global de raton con `SetWindowsHookEx` para clics.
- `MouseTrailRenderer`: estado temporal de puntos y marcas de clic.
- `TrackerSettings`: color y duracion.

## Decisiones iniciales

- Tracking desactivado al arrancar.
- Color por defecto: cian.
- Duracion por defecto de estela: 700 ms.
- El overlay debe ser click-through para no interferir con el raton.
- La configuracion no se persiste en disco en esta feature.

## Archivos esperados

- `MouseTracker.sln`
- `MouseTracker.App/MouseTracker.App.csproj`
- `MouseTracker.App/App.xaml`
- `MouseTracker.App/App.xaml.cs`
- `MouseTracker.App/OverlayWindow.xaml`
- `MouseTracker.App/OverlayWindow.xaml.cs`
- `MouseTracker.App/SettingsWindow.xaml`
- `MouseTracker.App/SettingsWindow.xaml.cs`
- `MouseTracker.App/Models/TrackerSettings.cs`
- `MouseTracker.App/Services/HotkeyService.cs`
- `MouseTracker.App/Services/MouseHookService.cs`
- `MouseTracker.App/Services/NativeMethods.cs`
- `MouseTracker.App/Rendering/MouseTrailRenderer.cs`

## Verificacion

- Compilar con `dotnet build MouseTracker.sln`.
- Ejecutar manualmente la app con `dotnet run --project MouseTracker.App`.
- Probar `Ctrl F9`, movimiento, clics y `Ctrl F10`.
