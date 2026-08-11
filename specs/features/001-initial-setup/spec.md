# Feature 001: Initial Setup

## Objetivo

Crear el primer prototipo funcional de mouseTracker como aplicacion local Windows con WPF. El prototipo debe permitir activar/desactivar un overlay visual con `Ctrl F9`, mostrar u ocultar una configuracion basica con `Ctrl F10`, dibujar una estela siguiendo el raton y mostrar una marca temporal cada vez que el usuario hace clic.

## Alcance

- Crear solucion `.sln` y proyecto WPF.
- Registrar hotkeys globales `Ctrl F9` y `Ctrl F10`. Son combinaciones de dos teclas; no se usa la tecla `+`.
- Leer posicion del raton en tiempo real.
- Detectar clics de raton mediante hook global.
- Dibujar una estela visual no interactiva.
- Dibujar marcas temporales de clic.
- Incluir ventana de configuracion para color y duracion.
- Incluir boton en configuracion para activar/desactivar la estela sin usar el teclado.

## Criterios de aceptacion

- `dotnet build MouseTracker.sln` termina correctamente.
- Al iniciar la app, el overlay existe pero el tracking empieza desactivado.
- `Ctrl F9` alterna entre tracking activo e inactivo.
- El boton `Activar estela` alterna entre tracking activo e inactivo.
- Con tracking activo, mover el raton genera una estela visible.
- Con tracking activo, hacer clic genera una marca visual temporal.
- `Ctrl F10` muestra u oculta la ventana de configuracion.
- La X de la ventana de configuracion cierra completamente la aplicacion.
- El boton `Ocultar` esconde solo la ventana de configuracion y mantiene el overlay disponible.
- Cambiar color en configuracion afecta a la estela y marcas nuevas.
- Cambiar duracion modifica el tiempo que la estela queda visible.
- El overlay no bloquea clics ni roba foco durante el uso normal.

## Fuera de alcance

- Soporte garantizado para pantalla completa exclusiva.
- Analitica de sesiones.
- Grabacion de pantalla.
- Persistencia de configuracion entre reinicios.
- Instalador o empaquetado final.

## Riesgos

- Los hooks globales pueden requerir tratamiento cuidadoso para no afectar al sistema.
- Algunos juegos pueden bloquear overlays o comportarse distinto en pantalla completa exclusiva.
- Un renderizado demasiado frecuente puede consumir CPU/GPU si no se controla.

## Verificacion manual

- Ejecutar la app.
- Pulsar `Ctrl F9` y comprobar que la estela aparece al mover el raton.
- Hacer varios clics y comprobar que se ven marcas temporales.
- Pulsar `Ctrl F10` para ocultar y volver a mostrar el menu; cambiar ajustes y comprobar que se aplican.
- Confirmar que se puede clicar en otras ventanas mientras el overlay esta activo.
