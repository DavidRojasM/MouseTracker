# mouseTracker

Aplicacion Windows para entrenamiento visual de raton. Muestra una estela del cursor en tiempo real y marcas temporales al hacer clic.

## Uso

- `Ctrl F9`: activa o desactiva la estela por defecto.
- `Ctrl F10`: muestra u oculta el menu de configuracion por defecto.
- Boton `Activar estela`: alternativa al atajo `Ctrl F9`.
- Boton `Ocultar`: oculta solo el menu.
- X de la ventana: cierra la aplicacion.
- Los atajos se pueden cambiar desde el menu de configuracion.
- Se permiten combinaciones y teclas sueltas, aunque las teclas sueltas pueden interferir al escribir o jugar.

## Configuracion local

La configuracion se guarda automaticamente en:

```text
%AppData%\mouseTracker\settings.json
```

Se persisten color, duracion de la estela y hotkeys.

## Ejecutar en desarrollo

```powershell
dotnet run --project MouseTracker.App
```

## Compilar

```powershell
dotnet build MouseTracker.sln
```

## Publicar version local

Requiere .NET Desktop Runtime instalado en el equipo destino.

```powershell
dotnet publish MouseTracker.App\MouseTracker.App.csproj -c Release -r win-x64 --self-contained false -o dist\MouseTracker
```

## Publicar version para compartir

No requiere instalar .NET en el equipo destino.

```powershell
dotnet publish MouseTracker.App\MouseTracker.App.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o dist\MouseTracker-self-contained
```

El ZIP final para compartir se genera desde `dist\MouseTracker-self-contained`.

Archivo actual listo para enviar:

```text
dist\mouseTracker-self-contained-win-x64.zip
```

El ejecutable final se llama `mouseTracker.exe`.

## Documentacion

- Constitucion SDD: `specs/constitution/`.
- Features: `specs/features/`.
