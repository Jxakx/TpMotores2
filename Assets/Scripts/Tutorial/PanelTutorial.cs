using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class PanelTutorial : MonoBehaviour
{
    public static PanelTutorial Instance;

    [Header("Referencias de UI Tutorial")]
    [SerializeField] private GameObject panelPrincipal;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TextMeshProUGUI textoTutorial;
    [SerializeField] private GameObject botonCerrar;

    [Header("Referencias de Gameplay")]
    [SerializeField] private GameObject[] canvasGameplay;

    private CartelTutorial cartelActual;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        videoPlayer.isLooping = true;

        // Nos aseguramos de que todo arranque apagado
        panelPrincipal.SetActive(false);
        botonCerrar.SetActive(false);
        textoTutorial.gameObject.SetActive(false);

        videoPlayer.loopPointReached += OnVideoLoopComplete;
    }

    public void AbrirTutorial(VideoClip clip, string texto, CartelTutorial cartel)
    {
        cartelActual = cartel;
        textoTutorial.text = texto;
        videoPlayer.clip = clip;

        // Forzamos al video a volver a cero
        videoPlayer.time = 0;
        videoPlayer.frame = 0;

        // 1. PRENDEMOS EL PANEL
        panelPrincipal.SetActive(true);

        // 2. PRENDEMOS EL TEXTO (Aparece junto con el video)
        textoTutorial.gameObject.SetActive(true);

        // 3. ASEGURAMOS QUE EL BOTÓN ESTÉ APAGADO
        botonCerrar.SetActive(false);

        // Ocultamos la UI del juego y silenciamos al Player
        foreach (GameObject canvas in canvasGameplay)
        {
            if (canvas != null) canvas.SetActive(false);
        }

        Player jugador = FindObjectOfType<Player>();
        if (jugador != null) jugador.SilenciarAudio();

        Time.timeScale = 0f;
        videoPlayer.Play();
    }

    private void OnVideoLoopComplete(VideoPlayer vp)
    {
        // 4. TERMINA EL PRIMER LOOP -> APARECE EL BOTÓN
        if (!botonCerrar.activeSelf)
        {
            botonCerrar.SetActive(true);
        }
    }

    public void CerrarTutorial()
    {
        Time.timeScale = 1f;
        videoPlayer.Stop();

        // 5. APAGAMOS ABSOLUTAMENTE TODO DE FORMA FORZADA
        botonCerrar.SetActive(false);
        textoTutorial.gameObject.SetActive(false);
        panelPrincipal.SetActive(false);

        // Volvemos a prender la UI del juego
        foreach (GameObject canvas in canvasGameplay)
        {
            if (canvas != null) canvas.SetActive(true);
        }

        if (cartelActual != null)
        {
            cartelActual.IniciarCooldown();
        }
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoLoopComplete;
        }
    }
}