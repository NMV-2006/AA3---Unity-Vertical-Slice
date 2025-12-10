using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Configuracion de Escenas")]
    [Tooltip("Nombre de la escena del juego principal")]
    public string nombreEscenaJuego = "SampleScene";

    [Header("Referencias UI (Opcional)")]
    public Button botonStart;
    public Button botonOptions;
    public Button botonExit;
    public GameObject panelOpciones;

    private void Start()
    {
        // Si las referencias no están asignadas, buscarlas automáticamente
        if (botonStart == null || botonOptions == null || botonExit == null)
        {
            AsignarBotonesAutomaticamente();
        }

        // Asignar listeners a los botones
        if (botonStart != null)
            botonStart.onClick.AddListener(IniciarJuego);
        
        if (botonOptions != null)
            botonOptions.onClick.AddListener(AbrirOpciones);
        
        if (botonExit != null)
            botonExit.onClick.AddListener(SalirJuego);

        // Asegurarse de que el panel de opciones esté oculto al inicio
        if (panelOpciones != null)
            panelOpciones.SetActive(false);
    }

    /// <summary>
    /// Inicia el juego cargando la escena principal
    /// </summary>
    public void IniciarJuego()
    {
        Debug.Log("Iniciando juego...");
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    /// <summary>
    /// Abre el panel de opciones
    /// </summary>
    public void AbrirOpciones()
    {
        Debug.Log("Abriendo opciones...");
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Panel de opciones no asignado. Por ahora solo muestra este mensaje.");
        }
    }

    /// <summary>
    /// Cierra el panel de opciones
    /// </summary>
    public void CerrarOpciones()
    {
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false);
        }
    }

    /// <summary>
    /// Sale del juego
    /// </summary>
    public void SalirJuego()
    {
        Debug.Log("Saliendo del juego...");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    /// <summary>
    /// Intenta encontrar los botones automáticamente por su nombre
    /// </summary>
    private void AsignarBotonesAutomaticamente()
    {
        Button[] todosLosBotones = FindObjectsOfType<Button>();
        
        foreach (Button boton in todosLosBotones)
        {
            string nombreBoton = boton.gameObject.name.ToLower();
            
            if (nombreBoton.Contains("start") || nombreBoton.Contains("iniciar") || nombreBoton.Contains("jugar"))
            {
                botonStart = boton;
            }
            else if (nombreBoton.Contains("option") || nombreBoton.Contains("opciones") || nombreBoton.Contains("settings"))
            {
                botonOptions = boton;
            }
            else if (nombreBoton.Contains("exit") || nombreBoton.Contains("salir") || nombreBoton.Contains("quit"))
            {
                botonExit = boton;
            }
        }
    }
}
