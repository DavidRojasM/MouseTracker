# Feature 002: Desktop Executable

## Objetivo

Permitir abrir mouseTracker desde el escritorio sin ejecutar `dotnet run` en una terminal.

## Alcance

- Publicar una build Release para Windows x64.
- Generar un ejecutable en `dist/MouseTracker/`.
- Crear un acceso directo en el escritorio del usuario apuntando al ejecutable publicado.
- Publicar una build self-contained para Windows x64 en `dist/MouseTracker-self-contained/`.
- Crear un ZIP compartible en `dist/mouseTracker-self-contained-win-x64.zip`.

## Criterios de aceptacion

- Existe `dist/MouseTracker/mouseTracker.exe`.
- Existe un acceso directo `mouseTracker.lnk` en el escritorio.
- El acceso directo apunta al ejecutable publicado.
- La publicacion termina sin errores.
- Existe `dist/MouseTracker-self-contained/mouseTracker.exe`.
- Existe `dist/mouseTracker-self-contained-win-x64.zip`.
- La version self-contained puede compartirse sin requerir instalar .NET aparte.

## Fuera de alcance

- Instalador MSI/MSIX.
- Icono personalizado.
- Inicio automatico con Windows.
