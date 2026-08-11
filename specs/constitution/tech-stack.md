# Tech Stack

## Estado

Stack elegido: .NET 8 SDK + C# + WPF.

El proyecto requiere una aplicacion local para Windows con tres capacidades tecnicas principales: atajos globales, lectura de posicion/clics del raton y renderizado de un overlay visual de baja latencia.

La version instalada y verificada es .NET SDK 8.0.422 con `Microsoft.WindowsDesktop.App` 8.0.28.

## Stack elegido

- Lenguaje: C#.
- Runtime/SDK: .NET 8 LTS.
- UI: WPF.
- Plataforma inicial: Windows x64.
- Tipo de proyecto: aplicacion de escritorio local.
- Build: `dotnet build MouseTracker.sln`.

## Requisitos tecnicos del stack

- Soporte para atajos globales: `Ctrl F9` y `Ctrl F10` deben funcionar aunque otra ventana este activa. No interviene la tecla `+`.
- Acceso a eventos de raton: posicion en tiempo real y eventos de clic.
- Overlay transparente o no intrusivo: debe dibujar encima del escritorio o aplicaciones sin capturar la interaccion del raton.
- Baja latencia: actualizacion visual fluida.
- Configuracion local: guardar preferencias si se decide persistirlas.
- Persistencia local: `%AppData%\mouseTracker\settings.json`.
- Empaquetado para Windows: instalable o ejecutable simple.

## Candidatos considerados

- C#/.NET + WPF/WinUI: fuerte integracion con Windows, buena opcion para hooks y overlay nativo. Elegido para el prototipo inicial.
- Electron + Node.js: desarrollo rapido con UI web, pero mayor consumo de recursos.
- Tauri + frontend web: mas ligero que Electron, requiere integracion nativa para hooks/overlay.
- Python + PySide/PyQt: prototipado rapido, posible mayor friccion para overlay robusto y distribucion.

## Criterios de decision

- Priorizar baja latencia y estabilidad durante juegos.
- Priorizar no interferir con los clics ni el foco de la ventana activa.
- Evitar consumo excesivo de CPU/GPU.
- Elegir una tecnologia que permita distribuir facilmente en Windows.
- No instalar dependencias adicionales hasta justificar la eleccion en una feature spec.

## Arquitectura recomendada

- Input tracking: modulo responsable de leer posicion del raton y clics.
- Hotkeys: modulo responsable de registrar y gestionar atajos globales.
- Overlay renderer: modulo responsable de dibujar estela y marcas de clic.
- Settings: modulo responsable de leer, aplicar y guardar configuracion.
- UI de configuracion: pantalla o ventana para modificar parametros visuales.

## Restricciones de seguridad

- No capturar pantalla por defecto.
- No registrar historiales persistentes de movimiento sin requisito explicito.
- No enviar datos fuera del equipo.
- No automatizar entradas de raton o teclado.

## Limitacion inicial

El primer prototipo se orienta a escritorio, aplicaciones normales y juegos en modo ventana/borderless. El comportamiento sobre juegos en pantalla completa exclusiva queda pendiente de validacion.
