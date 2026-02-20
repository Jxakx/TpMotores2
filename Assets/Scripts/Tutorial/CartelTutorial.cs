using UnityEngine;
using UnityEngine.Video;

public class CartelTutorial : MonoBehaviour
{
    [Header("Contenido de ESTE Tutorial")]
    [SerializeField] private VideoClip videoTutorial;
    [TextArea(3, 5)] // Esto hace que la cajita de texto en el Inspector sea más grande y cómoda
    [SerializeField] private string textoTutorial;

    [Header("Configuración de Cooldown")]
    [SerializeField] private float cooldownTiempo = 4f;
    private float proximoTiempoPermitido = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Usamos Time.unscaledTime porque Time.time se detiene cuando pausamos el juego a 0f
            if (Time.unscaledTime >= proximoTiempoPermitido)
            {
                // Llamamos al cerebro central y le pasamos nuestro video, texto y a nosotros mismos (this)
                PanelTutorial.Instance.AbrirTutorial(videoTutorial, textoTutorial, this);
            }
        }
    }

    // Esta función es llamada por el PanelTutorial justo cuando el jugador aprieta "Cerrar"
    public void IniciarCooldown()
    {
        // Sumamos 4 segundos al tiempo real actual para bloquear el trigger
        proximoTiempoPermitido = Time.unscaledTime + cooldownTiempo;
    }
}