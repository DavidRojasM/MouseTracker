# Feature 003: Custom Icon

## Objetivo

Dar a mouseTracker una identidad visual propia mediante un icono personalizado que aparezca en el ejecutable y en la ventana de configuracion.

## Alcance

- Crear `MouseTracker.App/Assets/mouseTracker.ico`.
- Configurar el proyecto para usar ese icono como icono de aplicacion.
- Mostrar el icono en `SettingsWindow`.
- Regenerar las publicaciones en `dist/` para que el ejecutable final incluya el icono.

## Criterios de aceptacion

- Existe `MouseTracker.App/Assets/mouseTracker.ico`.
- `MouseTracker.App.csproj` define `ApplicationIcon`.
- La ventana de configuracion usa el icono personalizado.
- `dotnet build MouseTracker.sln` termina sin errores.
- La version publicada mantiene el icono en `mouseTracker.exe`.

## Fuera de alcance

- Disenar multiples variantes de marca.
- Instalador con accesos directos avanzados.
