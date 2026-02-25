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
    private Vector3 escalaOriginalVideo;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        videoPlayer.isLooping = true;
        videoPlayer.timeUpdateMode = VideoTimeUpdateMode.UnscaledGameTime;

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

        ButtonController btnController = FindObjectOfType<ButtonController>();
        if (btnController != null)
        {
            btnController.stopMovement();
        }

        foreach (GameObject canvas in canvasGameplay)
        {
            if (canvas != null) canvas.SetActive(false);
        }

        Player jugador = FindObjectOfType<Player>();
        if (jugador != null) jugador.SilenciarAudio();

        // Llamamos a la corrutina que prepara el video de forma segura
        StartCoroutine(RutinaPrepararYMostrar(clip));
    }

    private IEnumerator RutinaPrepararYMostrar(VideoClip clip)
    {
        // 1. Asignamos el video y lo mandamos a preparar MIENTRAS EL TIEMPO FLUYE NORMAL
        videoPlayer.clip = clip;
        videoPlayer.Prepare();

        // 2. Esperamos hasta que el celular termine de cargar el video en la memoria
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // 3. ¡Recién ahora pausamos el juego! Así evitamos la cámara lenta.
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
            videoObjeto.transform.localScale = Vector3.Lerp(Vector3.zero, escalaOriginalVideo, progreso);
            yield return null;
        }

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
            videoObjeto.transform.localScale = Vector3.Lerp(escalaOriginalVideo, Vector3.zero, progreso);
            yield return null;
        }

        // --- EL SECRETO ANTI-CONGELAMIENTO ---
        // 1. Primero apagamos lo visual para que el jugador pueda seguir jugando pase lo que pase
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

        // 2. Despausamos el juego
        Time.timeScale = 1f;

        // 3. Le damos al celular un respiro de 0.1 segundos REALES
        yield return new WaitForSecondsRealtime(0.1f);

        // 4. Apagamos el video de forma segura usando Pause en vez de Stop
        videoPlayer.Pause();
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoLoopComplete;
        }
    }
}