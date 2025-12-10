using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsPanel : MonoBehaviour
{
    [Header("Referencias UI")]
    public Slider volumenSlider;
    public Toggle fullscreenToggle;
    public Dropdown resolucionDropdown;
    public Button botonCerrar;

    [Header("Audio")]
    public AudioMixer audioMixer;

    private Resolution[] resoluciones;

    private void Start()
    {
        // Configurar resoluciones disponibles
        ConfigurarResoluciones();

        // Cargar configuración guardada
        CargarConfiguracion();

        // Asignar listeners
        if (volumenSlider != null)
            volumenSlider.onValueChanged.AddListener(CambiarVolumen);

        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(CambiarPantallaCompleta);

        if (resolucionDropdown != null)
            resolucionDropdown.onValueChanged.AddListener(CambiarResolucion);

        if (botonCerrar != null)
            botonCerrar.onClick.AddListener(CerrarPanel);
    }

    private void ConfigurarResoluciones()
    {
        if (resolucionDropdown == null) return;

        resoluciones = Screen.resolutions;
        resolucionDropdown.ClearOptions();

        System.Collections.Generic.List<string> opciones = new System.Collections.Generic.List<string>();
        int resolucionActualIndex = 0;

        for (int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + " x " + resoluciones[i].height;
            opciones.Add(opcion);

            if (resoluciones[i].width == Screen.currentResolution.width &&
                resoluciones[i].height == Screen.currentResolution.height)
            {
                resolucionActualIndex = i;
            }
        }

        resolucionDropdown.AddOptions(opciones);
        resolucionDropdown.value = resolucionActualIndex;
        resolucionDropdown.RefreshShownValue();
    }

    public void CambiarVolumen(float volumen)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volumen) * 20);
        }
        else
        {
            AudioListener.volume = volumen;
        }
        PlayerPrefs.SetFloat("Volumen", volumen);
    }

    public void CambiarPantallaCompleta(bool esCompleta)
    {
        Screen.fullScreen = esCompleta;
        PlayerPrefs.SetInt("Fullscreen", esCompleta ? 1 : 0);
    }

    public void CambiarResolucion(int indiceResolucion)
    {
        if (resoluciones != null && indiceResolucion < resoluciones.Length)
        {
            Resolution resolucion = resoluciones[indiceResolucion];
            Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
            PlayerPrefs.SetInt("ResolucionIndex", indiceResolucion);
        }
    }

    private void CargarConfiguracion()
    {
        // Cargar volumen
        if (volumenSlider != null)
        {
            float volumen = PlayerPrefs.GetFloat("Volumen", 1f);
            volumenSlider.value = volumen;
            CambiarVolumen(volumen);
        }

        // Cargar pantalla completa
        if (fullscreenToggle != null)
        {
            bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
            fullscreenToggle.isOn = fullscreen;
        }

        // Cargar resolución
        if (resolucionDropdown != null)
        {
            int resIndex = PlayerPrefs.GetInt("ResolucionIndex", resoluciones.Length - 1);
            resolucionDropdown.value = resIndex;
        }
    }

    public void CerrarPanel()
    {
        gameObject.SetActive(false);
    }

    public void RestaurarDefecto()
    {
        if (volumenSlider != null)
            volumenSlider.value = 1f;

        if (fullscreenToggle != null)
            fullscreenToggle.isOn = true;

        if (resolucionDropdown != null && resoluciones != null)
            resolucionDropdown.value = resoluciones.Length - 1;

        PlayerPrefs.DeleteAll();
    }
}
