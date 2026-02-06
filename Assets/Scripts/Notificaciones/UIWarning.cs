using UnityEngine;
using TMPro;
using System.Collections;

public class UIWarning : MonoBehaviour
{
    [SerializeField] private GameObject panelAviso; // El objeto del Pop-up
    [SerializeField] private AudioSource audioError; // El sonido de "clinc"

    public void MostrarAviso()
    {
        StopAllCoroutines(); // Por si el usuario clickea muchas veces
        StartCoroutine(RutinaAviso());
    }

    private IEnumerator RutinaAviso()
    {
        panelAviso.SetActive(true);
        if (audioError != null) audioError.Play();

        yield return new WaitForSecondsRealtime(3f); // Espera 3 segundos (sin importar el TimeScale)

        panelAviso.SetActive(false);
    }
}