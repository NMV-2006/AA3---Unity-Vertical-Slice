# 🎮 Cómo Crear Botones en Unity - Guía Paso a Paso

## Paso 1: Crear una Nueva Escena para el Menú

1. **Abre Unity**
2. En la parte superior, ve a: **File → New Scene**
3. Selecciona **"Basic (Built-in)"** o **"2D"**
4. Click en **Create**
5. Guarda la escena:
   - **File → Save As...**
   - Navega a la carpeta **Assets/Scenes/**
   - Nombra la escena: **MainMenu**
   - Click en **Save**

---

## Paso 2: Crear el Canvas (Lienzo)

El Canvas es el contenedor donde irán todos los elementos de UI (botones, texto, imágenes, etc.)

### Crear el Canvas:
1. En la ventana **Hierarchy** (Jerarquía, normalmente a la izquierda):
   - **Click derecho** en un espacio vacío
   - Selecciona: **UI → Canvas**

### ¿Qué se creó?
Verás que se crearon **2 objetos** automáticamente:
- ✅ **Canvas** - El lienzo donde irán tus botones
- ✅ **EventSystem** - Necesario para que los botones funcionen (¡no lo borres!)

### Configurar el Canvas (IMPORTANTE):
1. **Selecciona el Canvas** en la Hierarchy
2. En el **Inspector** (panel derecho), busca el componente **Canvas Scaler**
3. Cambia estos valores:
   - **UI Scale Mode**: Cambia a **"Scale With Screen Size"**
   - **Reference Resolution**: 
     - X: **1920**
     - Y: **1080**
   - **Screen Match Mode**: **Match Width Or Height**
   - **Match**: **0.5** (en el medio)

Esto hace que tu menú se vea bien en diferentes tamaños de pantalla.

---

## Paso 3: Crear un Panel de Fondo (Opcional pero Recomendado)

Un panel de fondo hace que tu menú se vea más profesional.

1. **Click derecho** en el **Canvas** en la Hierarchy
2. Selecciona: **UI → Panel**
3. Renómbralo a **"Background"**:
   - Selecciona el Panel
   - En el Inspector, cambia el nombre en la parte superior
   - O presiona **F2** para renombrar rápidamente

### Personalizar el Fondo:
1. Con el **Background** seleccionado
2. En el Inspector, busca el componente **Image**
3. Cambia el **Color** a tu gusto (por ejemplo, un azul oscuro o negro)

---

## Paso 4: Crear los 3 Botones

Ahora viene la parte importante: crear los botones.

### Crear el Primer Botón (START):

1. **Click derecho** en el **Canvas** en la Hierarchy
2. Selecciona: **UI → Button - TextMeshPro**
   - Si aparece una ventana diciendo "Import TMP Essentials", click en **"Import TMP Essentials"** y espera
   - Si no tienes TextMeshPro, selecciona **UI → Button** (normal)

3. **Renombrar el botón**:
   - Selecciona el botón que acabas de crear
   - En el Inspector (arriba), cambia el nombre a: **ButtonStart**
   - O presiona **F2** y escribe **ButtonStart**

4. **Posicionar el botón**:
   - Con **ButtonStart** seleccionado
   - En el Inspector, busca **Rect Transform**
   - Cambia estos valores:
     - **Pos X**: **0**
     - **Pos Y**: **100** (para que esté arriba)
     - **Width**: **250**
     - **Height**: **70**

5. **Cambiar el texto del botón**:
   - En la Hierarchy, **expande ButtonStart** (click en la flechita)
   - Verás un objeto hijo llamado **"Text (TMP)"** o **"Text"**
   - **Selecciónalo**
   - En el Inspector, busca el campo **Text** o **Text Input**
   - Cambia el texto a: **START** o **JUGAR**
   - Cambia el **Font Size** a **32** o **36**

### Crear el Segundo Botón (OPTIONS):

1. **Click derecho** en el **Canvas**
2. Selecciona: **UI → Button - TextMeshPro** (o **UI → Button**)
3. Renómbralo a: **ButtonOptions**
4. Configura el **Rect Transform**:
   - **Pos X**: **0**
   - **Pos Y**: **0** (en el centro)
   - **Width**: **250**
   - **Height**: **70**
5. Cambia el texto:
   - Expande **ButtonOptions**
   - Selecciona el **Text**
   - Cambia el texto a: **OPTIONS** o **OPCIONES**
   - Font Size: **32** o **36**

### Crear el Tercer Botón (EXIT):

1. **Click derecho** en el **Canvas**
2. Selecciona: **UI → Button - TextMeshPro** (o **UI → Button**)
3. Renómbralo a: **ButtonExit**
4. Configura el **Rect Transform**:
   - **Pos X**: **0**
   - **Pos Y**: **-100** (para que esté abajo)
   - **Width**: **250**
   - **Height**: **70**
5. Cambia el texto:
   - Expande **ButtonExit**
   - Selecciona el **Text**
   - Cambia el texto a: **EXIT** o **SALIR**
   - Font Size: **32** o **36**

---

## Paso 5: Crear el Panel de Opciones

Este panel se mostrará cuando hagas click en "Options".

1. **Click derecho** en el **Canvas**
2. Selecciona: **UI → Panel**
3. Renómbralo a: **OptionsPanel**
4. **IMPORTANTE**: Con el **OptionsPanel** seleccionado:
   - En el Inspector, **desmarca el checkbox** al lado del nombre
   - Esto desactiva el panel para que esté oculto al inicio

### Añadir un Botón de Cerrar al Panel:

1. **Click derecho** en **OptionsPanel**
2. Selecciona: **UI → Button - TextMeshPro**
3. Renómbralo a: **ButtonClose**
4. Posiciónalo en una esquina:
   - **Pos X**: **400** (esquina derecha)
   - **Pos Y**: **250** (arriba)
   - **Width**: **150**
   - **Height**: **60**
5. Cambia el texto a: **CERRAR** o **VOLVER**

---

## Paso 6: Conectar el Script MainMenuManager

Ahora vamos a hacer que los botones funcionen.

### Añadir el Script:

1. En la Hierarchy, **selecciona el Canvas**
2. En el Inspector, ve hasta abajo
3. Click en **Add Component**
4. Escribe: **MainMenuManager**
5. Selecciona el script **MainMenuManager**

### Conectar los Botones:

Con el **Canvas** seleccionado, verás el componente **MainMenuManager** en el Inspector.

1. **Nombre Escena Juego**: Escribe **"SampleScene"** (o el nombre de tu escena de juego)

2. **Boton Start**: 
   - Click en el círculo pequeño a la derecha
   - O arrastra **ButtonStart** desde la Hierarchy

3. **Boton Options**: 
   - Arrastra **ButtonOptions** desde la Hierarchy

4. **Boton Exit**: 
   - Arrastra **ButtonExit** desde la Hierarchy

5. **Panel Opciones**: 
   - Arrastra **OptionsPanel** desde la Hierarchy

---

## Paso 7: Añadir las Escenas al Build

Para que el botón "Start" funcione, necesitas añadir las escenas al Build.

1. Ve a: **File → Build Settings**
2. Verás una ventana con **"Scenes In Build"**
3. **Arrastra** la escena **MainMenu** desde la carpeta Scenes a esta lista
4. **Arrastra** la escena **SampleScene** (tu juego) a esta lista
5. **IMPORTANTE**: **MainMenu** debe estar **primero** (índice 0)
6. Cierra la ventana

---

## Paso 8: ¡Probar el Menú!

1. Asegúrate de que estás en la escena **MainMenu**
2. Click en el botón **Play** ▶️ (arriba en el centro)
3. Prueba los botones:
   - ✅ **START** → Debería cargar tu juego
   - ✅ **OPTIONS** → Debería mostrar el panel de opciones
   - ✅ **EXIT** → Debería parar el juego en el editor

---

## 🎨 Personalización Extra (Opcional)

### Cambiar Colores de los Botones:

1. Selecciona un botón (por ejemplo, **ButtonStart**)
2. En el Inspector, busca el componente **Button**
3. Verás una sección **"Colors"**:
   - **Normal Color**: Color normal del botón
   - **Highlighted Color**: Color cuando pasas el mouse
   - **Pressed Color**: Color cuando haces click
   - **Selected Color**: Color cuando está seleccionado
4. Click en cada color y elige el que quieras

### Hacer los Botones más Bonitos:

1. Selecciona un botón
2. En el componente **Image**, cambia:
   - **Color**: El color de fondo del botón
   - **Material**: Puedes añadir efectos especiales
3. Para el texto:
   - Selecciona el objeto **Text** hijo
   - Cambia **Font**: Puedes importar fuentes de Google Fonts
   - Cambia **Color**: Color del texto
   - Añade **Outline**: Para un borde alrededor del texto

### Añadir una Imagen de Fondo:

1. Importa una imagen a Unity (arrastra una imagen a la carpeta Assets)
2. Selecciona el **Background** panel
3. En el componente **Image**:
   - Click en el círculo al lado de **Source Image**
   - Selecciona tu imagen

---

## ❓ Solución de Problemas

### "No puedo hacer click en los botones"
- ✅ Asegúrate de que hay un **EventSystem** en la Hierarchy
- ✅ Verifica que el **Canvas** tiene el componente **Graphic Raycaster**

### "El botón Start no hace nada"
- ✅ Verifica que el nombre de la escena en **MainMenuManager** es correcto
- ✅ Asegúrate de que ambas escenas están en **Build Settings**

### "Los botones se ven muy pequeños o muy grandes"
- ✅ Verifica la configuración del **Canvas Scaler**
- ✅ Ajusta el **Width** y **Height** en el Rect Transform

### "El panel de opciones no se cierra"
- ✅ Asegúrate de que el **ButtonClose** tiene el script configurado
- ✅ Verifica que el **OptionsPanel** está asignado en el MainMenuManager

---

## 📋 Resumen Rápido

1. ✅ Crear nueva escena "MainMenu"
2. ✅ Añadir Canvas (UI → Canvas)
3. ✅ Crear 3 botones (UI → Button)
4. ✅ Renombrar: ButtonStart, ButtonOptions, ButtonExit
5. ✅ Cambiar el texto de cada botón
6. ✅ Crear OptionsPanel (UI → Panel) y desactivarlo
7. ✅ Añadir MainMenuManager al Canvas
8. ✅ Conectar los botones en el Inspector
9. ✅ Añadir escenas a Build Settings
10. ✅ ¡Probar!

---

¡Listo! Ahora tienes un menú principal funcional. 🎉
