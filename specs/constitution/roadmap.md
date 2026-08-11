# Roadmap

## Fase 0: Constitucion del proyecto

- Crear `specs/constitution/mission.md`.
- Crear `specs/constitution/tech-stack.md`.
- Crear `specs/constitution/roadmap.md`.
- Validar stack tecnico inicial antes de implementar: .NET 8 + C# + WPF.

## Fase 1: Prototipo minimo

- Crear una aplicacion local para Windows.
- Detectar posicion del raton en tiempo real.
- Dibujar una estela basica siguiendo el cursor.
- Mostrar marcas temporales al hacer clic.
- Activar/desactivar seguimiento con `Ctrl F9`.

## Fase 2: Configuracion basica

- Mostrar u ocultar menu con `Ctrl F10`.
- Permitir cambiar duracion/longitud de la estela.
- Permitir cambiar color de la estela.
- Permitir cambiar hotkeys globales.
- Guardar configuracion entre reinicios.
- Aplicar cambios sin reiniciar la aplicacion.

## Fase 3: Pulido para uso real

- Mejorar rendimiento y suavidad visual.
- Evitar que el overlay robe foco o bloquee clics.
- Definir comportamiento con juegos en ventana, borderless y pantalla completa.
- Guardar configuracion local entre sesiones si se valida como requisito.

## Fase 4: Experiencia de usuario

- Icono personalizado de aplicacion.
- Icono en bandeja del sistema si el stack lo permite.
- Indicador visual del estado activo/inactivo.
- Presets de colores y duraciones.
- Diferenciar marcas por tipo de clic si se valida como requisito.

## Fase futura: Analitica opcional

- Contador de clics por minuto.
- Mapas de movimiento o zonas frecuentes.
- Sesiones de entrenamiento con resumen local.
- Exportacion local de datos anonimos si el usuario lo solicita.

## Estado actual

El prototipo inicial, icono personalizado, hotkeys configurables, persistencia local y empaquetado estan completados. La version compartible esta en `dist/mouseTracker-self-contained-win-x64.zip`.
