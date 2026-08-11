# Feature 004: Configurable Hotkeys And Persistence

## Objetivo

Permitir que cada usuario configure los atajos globales de mouseTracker y que la configuracion se conserve al reiniciar la aplicacion.

## Alcance

- Mantener como valores por defecto `Ctrl F9` para activar/desactivar estela y `Ctrl F10` para mostrar/ocultar menu.
- Permitir cambiar cada hotkey desde la ventana de configuracion.
- Soportar combinaciones con modificadores (`Ctrl`, `Alt`, `Shift`, `Win`) y teclas sueltas.
- Guardar configuracion local en `%AppData%/mouseTracker/settings.json`.
- Persistir duracion de estela, color y hotkeys.
- Cargar configuracion al arrancar.
- Si el archivo de configuracion no existe o esta corrupto, usar valores por defecto.

## Criterios de aceptacion

- Al arrancar por primera vez se usan `Ctrl F9` y `Ctrl F10`.
- La UI muestra los atajos activos.
- El usuario puede seleccionar una tecla suelta como hotkey.
- El usuario puede seleccionar una combinacion con modificadores como hotkey.
- No se permite usar la misma hotkey para las dos acciones.
- Si Windows rechaza una hotkey por estar ocupada, se mantiene la configuracion anterior.
- Al cerrar y abrir la app, se conserva la configuracion elegida.
- `dotnet build MouseTracker.sln` termina sin errores.

## Fuera de alcance

- Sincronizacion cloud.
- Perfiles por juego.
- Importar/exportar configuraciones.

## Riesgos

- Las teclas sueltas globales pueden interferir con escritura o juegos.
- Algunas hotkeys pueden estar reservadas por Windows u otras aplicaciones.
