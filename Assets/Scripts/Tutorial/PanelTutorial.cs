using UnityEngine;
using UnityEngine.Video;
using TMPro;
using System.Collections;

public class PanelTutorial : MonoBehaviour
{
    public static PanelTutorial Instance;

    [Header("Referencias de UI Tutorial")]
    [SerializeField] private GameObject panelPrincipal;
    [SerializeField] private GameObject videoObjeto;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TextMeshProUGUI textoTutorial;
    [SerializeField] private GameObject botonCerrar;

    [Header("Referencias de Gameplay")]
    [SerializeField] private GameObject[] canvasGameplay;

    [Header("Animación")]
    [SerializeField] private float tiempoAnimacion = 0.25f;

    private CartelTutorial cartelActual;

    // NUEVA VARIABLE: Guarda el tamaño que le diste en Unity
    private Vector3 escalaOriginalVideo;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        videoPlayer.isLooping = true;

        // ¡MAGIA! Memorizamos tu escala personalizada antes de apagar nada
        if (videoObjeto != null)
        {
            escalaOriginalVideo = videoObjeto.transform.localScale;
        }

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

        videoPlayer.time = 0;
        videoPlayer.frame = 0;

        foreach (GameObject canvas in canvasGameplay)
        {
            if (canvas != null) canvas.SetActive(false);
        }

        Player jugador = FindObjectOfType<Player>();
        if (jugador != null) jugador.SilenciarAudio();

        Time.timeScale = 0f;
        videoPlayer.Play();

        StartCoroutine(AnimacionAparecer());
    }

    private IEnumerator AnimacionAparecer()
    {
        panelPrincipal.SetActive(true);
        textoTutorial.gameObject.SetActive(true);
        botonCerrar.SetActive(false);

        videoObjeto.transform.localScale = Vector3.zero;
        videoObjeto.SetActive(true);

        float tiempo = 0f;
        while (tiempo < tiempoAnimacion)
        {
            tiempo += Time.unscaledDeltaTime;
            float progreso = tiempo / tiempoAnimacion;

            // Usamos tu escala original en vez de Vector3.one
            videoObjeto.transform.localScale = Vector3.Lerp(Vector3.zero, escalaOriginalVideo, progreso);
            yield return null;
        }

        // Lo dejamos exactamente del tamaño que configuraste
        videoObjeto.transform.localScale = escalaOriginalVideo;
    }

    private void OnVideoLoopComplete(VideoPlayer vp)
    {
        if (!botonCerrar.activeSelf)
        {
            botonCerrar.SetActive(true);
        }
    }

    public void CerrarTutorial()
    {
        StartCoroutine(AnimacionDesaparecer());
    }

    private IEnumerator AnimacionDesaparecer()
    {
        botonCerrar.SetActive(false);
        textoTutorial.gameObject.SetActive(false);

        float tiempo = 0f;
        while (tiempo < tiempoAnimacion)
        {
            tiempo += Time.unscaledDeltaTime;
            float progreso = tiempo / tiempoAnimacion;

            // Usamos tu escala original en vez de Vector3.one
            videoObjeto.transform.localScale = Vector3.Lerp(escalaOriginalVideo, Vector3.zero, progreso);
            yield return null;
        }

        Time.timeScale = 1f;
        videoPlayer.Stop();

        videoObjeto.SetActive(false);
        panelPrincipal.SetActive(false);

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