using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoFPS;

    private float temporizador;
    private float tiempoDeActualizacion = 0.25f;

    private void Update()
    {
        // Solo actualizamos el texto si ya pasó el tiempo de espera
        if (Time.unscaledTime > temporizador)
        {
            // Calculamos los FPS dividiendo 1 segundo por el tiempo que tardó el último frame
            int fps = Mathf.RoundToInt(1f / Time.unscaledDeltaTime);

            // Lo mostramos en pantalla
            textoFPS.text = "" + fps;

            // Seteamos el temporizador para la próxima actualización
            temporizador = Time.unscaledTime + tiempoDeActualizacion;
        }
    }
}