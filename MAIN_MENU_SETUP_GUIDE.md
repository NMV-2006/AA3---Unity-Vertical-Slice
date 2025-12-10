# Guía de Configuración del Menú Principal

## Scripts Creados
1. **MainMenuManager.cs** - Gestiona los botones principales del menú
2. **OptionsPanel.cs** - Gestiona el panel de opciones

## Pasos para Configurar el Menú Principal en Unity

### 1. Crear la Escena del Menú Principal
1. En Unity, ve a `File > New Scene`
2. Guarda la escena como `MainMenu` en la carpeta `Assets/Scenes/`

### 2. Crear el Canvas y los Botones

#### A. Crear el Canvas
1. Click derecho en la Jerarquía → `UI > Canvas`
2. Esto creará automáticamente un Canvas y un EventSystem

#### B. Configurar el Canvas
1. Selecciona el Canvas
2. En el Inspector, configura:
   - **Render Mode**: Screen Space - Overlay
   - **Canvas Scaler**: Scale With Screen Size
   - **Reference Resolution**: 1920 x 1080

#### C. Crear un Panel de Fondo (Opcional pero recomendado)
1. Click derecho en Canvas → `UI > Panel`
2. Renómbralo como "Background"
3. Puedes cambiar el color o añadir una imagen de fondo

#### D. Crear los Botones Principales
1. Click derecho en Canvas → `UI > Button - TextMeshPro` (o `UI > Button` si no tienes TextMeshPro)
2. Crea 3 botones y renómbralos:
   - **ButtonStart**
   - **ButtonOptions**
   - **ButtonExit**

#### E. Posicionar los Botones
1. Selecciona cada botón y ajusta su posición:
   - Usa el Rect Transform para centrarlos
   - Ejemplo de posiciones Y:
     - ButtonStart: Y = 50
     - ButtonOptions: Y = -50
     - ButtonExit: Y = -150
   - Tamaño recomendado: Width = 200, Height = 60

#### F. Cambiar el Texto de los Botones
1. Expande cada botón en la jerarquía
2. Selecciona el objeto "Text" hijo
3. Cambia el texto:
   - ButtonStart → "START" o "JUGAR"
   - ButtonOptions → "OPTIONS" o "OPCIONES"
   - ButtonExit → "EXIT" o "SALIR"
4. Ajusta el tamaño de fuente (recomendado: 24-32)

### 3. Crear el Panel de Opciones

#### A. Crear el Panel
1. Click derecho en Canvas → `UI > Panel`
2. Renómbralo como "OptionsPanel"
3. Ajusta el tamaño para que cubra toda la pantalla o parte de ella
4. **IMPORTANTE**: Desactiva el panel (checkbox en el Inspector) para que esté oculto al inicio

#### B. Añadir Elementos al Panel de Opciones (Opcional)
1. **Slider de Volumen**:
   - Click derecho en OptionsPanel → `UI > Slider`
   - Renombrar como "VolumeSlider"
   - Añadir un Text arriba que diga "Volumen"

2. **Toggle de Pantalla Completa**:
   - Click derecho en OptionsPanel → `UI > Toggle`
   - Renombrar como "FullscreenToggle"
   - Añadir un Text que diga "Pantalla Completa"

3. **Botón Cerrar**:
   - Click derecho en OptionsPanel → `UI > Button`
   - Renombrar como "ButtonClose"
   - Cambiar el texto a "CERRAR" o "VOLVER"

### 4. Configurar el MainMenuManager

#### A. Añadir el Script al Canvas
1. Selecciona el Canvas
2. En el Inspector, click en "Add Component"
3. Busca y añade "MainMenuManager"

#### B. Configurar las Referencias
1. Con el Canvas seleccionado, verás el componente MainMenuManager
2. Arrastra los objetos desde la jerarquía a los campos correspondientes:
   - **Nombre Escena Juego**: Escribe "SampleScene" (o el nombre de tu escena de juego)
   - **Boton Start**: Arrastra ButtonStart
   - **Boton Options**: Arrastra ButtonOptions
   - **Boton Exit**: Arrastra ButtonExit
   - **Panel Opciones**: Arrastra OptionsPanel

**NOTA**: Si no asignas los botones manualmente, el script intentará encontrarlos automáticamente por nombre.

### 5. Configurar el OptionsPanel (Opcional)

Si creaste elementos en el panel de opciones:
1. Selecciona el OptionsPanel
2. Añade el componente "OptionsPanel"
3. Arrastra las referencias:
   - **Volumen Slider**: Tu slider de volumen
   - **Fullscreen Toggle**: Tu toggle de pantalla completa
   - **Boton Cerrar**: El botón de cerrar

### 6. Añadir las Escenas al Build Settings

1. Ve a `File > Build Settings`
2. Arrastra ambas escenas a la lista:
   - **MainMenu** (debe ser la primera, índice 0)
   - **SampleScene** (índice 1)
3. Cierra la ventana

### 7. Probar el Menú

1. Abre la escena MainMenu
2. Dale a Play
3. Prueba cada botón:
   - **Start**: Debería cargar la escena del juego
   - **Options**: Debería abrir el panel de opciones
   - **Exit**: Debería cerrar el juego (en el editor solo para la reproducción)

## Personalización Adicional

### Mejorar la Apariencia
1. **Colores de los Botones**:
   - Selecciona cada botón
   - En el componente Button, ajusta los colores:
     - Normal Color
     - Highlighted Color
     - Pressed Color

2. **Añadir Imágenes de Fondo**:
   - Importa una imagen a Unity
   - Arrástrala al componente Image del Background Panel

3. **Añadir Efectos de Transición**:
   - En el componente Button, cambia "Transition" a "Animation" para efectos más avanzados

### Volver al Menú Principal desde el Juego
Si quieres un botón de pausa que vuelva al menú:
```csharp
using UnityEngine.SceneManagement;

public void VolverAlMenu()
{
    SceneManager.LoadScene("MainMenu");
}
```

## Solución de Problemas

- **Los botones no funcionan**: Asegúrate de que hay un EventSystem en la escena
- **El botón Start no carga la escena**: Verifica que el nombre de la escena en MainMenuManager coincide exactamente con el nombre en Build Settings
- **El panel de opciones no se cierra**: Asegúrate de que el botón de cerrar tiene asignado el método CerrarOpciones() del MainMenuManager

## Próximos Pasos Recomendados

1. Añadir música de fondo al menú
2. Añadir efectos de sonido a los botones
3. Crear animaciones para la entrada del menú
4. Añadir más opciones (controles, gráficos, etc.)
5. Crear un sistema de pausa en el juego
