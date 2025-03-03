using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] private Slider loadBar;
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private float fakeLoadTime = 3f; // Tiempo extra para completar la barra

    private JSONSaveHandler saveHandler; // Referencia al JSONSaveHandler

    private void Start()
    {
        saveHandler = FindObjectOfType<JSONSaveHandler>(); // Buscar el JSONSaveHandler al inicio
    }

    // Método público para cargar un nivel con verificación de estrellas
    public void LoadLevelWithStars(int sceneIndex, int requiredStars)
    {
        // Verificar si el jugador tiene suficientes estrellas
        int starsLevelOne = saveHandler.LoadStars(1); // Cargar estrellas del nivel 1

        if (starsLevelOne >= requiredStars)
        {
            // Si tiene suficientes estrellas, iniciar la carga del nivel
            StartCoroutine(LoadAsync(sceneIndex));
        }
        else
        {
            // Si no tiene suficientes estrellas, mostrar un mensaje de debug
            Debug.Log("No tienes suficientes estrellas para desbloquear este nivel.");
        }
    }

    // Corrutina para cargar la escena de manera asíncrona
    private IEnumerator LoadAsync(int sceneIndex)
    {
        loadPanel.SetActive(true); // Activar el panel de carga
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false; // Evitar que la escena se cargue automáticamente

        float progress = 0f;

        while (asyncOperation.progress < 0.9f) // Mientras la carga real no se complete
        {
            progress = asyncOperation.progress / 0.9f;
            loadBar.value = progress;
            yield return null;
        }

        // Simulación de los últimos segundos de carga
        float elapsedTime = 0f;
        while (elapsedTime < fakeLoadTime)
        {
            elapsedTime += Time.deltaTime;
            loadBar.value = Mathf.Lerp(progress, 1f, elapsedTime / fakeLoadTime); // Transición suave de la barra
            yield return null;
        }

        // Una vez terminado el tiempo extra, se activa la escena
        asyncOperation.allowSceneActivation = true;
    }
}
