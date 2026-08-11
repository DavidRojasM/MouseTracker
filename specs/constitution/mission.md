# Mision de mouseTracker

## Proposito

mouseTracker existe para ayudar a jugadores a entrenar y analizar visualmente sus movimientos de raton durante sesiones de juego o practica.

La aplicacion muestra una estela visual en tiempo real siguiendo el cursor y marca los clics con pequenos indicadores temporales. La finalidad es aumentar la consciencia del jugador sobre su trayectoria, precision y frecuencia de clics, especialmente en juegos donde el control del raton es critico, como League of Legends.

## Usuarios objetivo

- Jugadores que quieren mejorar su mecanica de raton.
- Jugadores de MOBAs, RTS, shooters tacticos u otros juegos con alta exigencia de precision.
- Personas que quieren observar patrones de movimiento y clic sin herramientas complejas de analitica.

## Problema que resuelve

Durante una partida, el jugador suele centrarse en el juego y no percibe con claridad como mueve el raton, si hace movimientos innecesarios, si clican demasiado, si corrige tarde o si arrastra malos habitos.

mouseTracker ofrece feedback visual inmediato sin alterar el comportamiento del raton ni interactuar con el juego.

## Alcance inicial

- Activar/desactivar seguimiento con `Ctrl F9`, usando solo Control y F9.
- Mostrar una estela visual del movimiento del raton en tiempo real.
- Mostrar una marca visual cada vez que el usuario hace clic.
- Mostrar u ocultar el menu de configuracion con `Ctrl F10`, usando solo Control y F10.
- Configurar longitud/duracion de la estela.
- Configurar color de la estela.
- Configurar hotkeys globales.
- Guardar configuracion entre reinicios.

## Fuera de alcance inicial

- Analiticas avanzadas de rendimiento.
- Grabacion de pantalla.
- Envio de datos a servidores externos.
- Integracion directa con APIs de videojuegos.
- Automatizacion de clics o movimientos.
- Cualquier funcionalidad que modifique, bloquee o sustituya la entrada real del usuario.

## Principio rector

La aplicacion debe observar y visualizar, no intervenir. El raton sigue perteneciendo siempre al usuario.
