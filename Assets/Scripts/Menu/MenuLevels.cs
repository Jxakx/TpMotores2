using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLevels : MonoBehaviour
{
    [Header("Referencias UI")]
    public Button levelTwoButton; // Arrastra el botón del Nivel 2 aquí
    public GameObject candadoNivel2; // Arrastra la imagen del candado aquí (opcional)

    private JSONSaveHandler saveHandler;
    private SceneLoadManager sceneLoadManager;

    [SerializeField] private UIWarning warningSystem;
    private const int COSTO_ESTAMINA = 5;

    private void Start()
    {
        saveHandler = FindObjectOfType<JSONSaveHandler>();
        sceneLoadManager = FindObjectOfType<SceneLoadManager>();

        VerificarDesbloqueo();
    }

    private void VerificarDesbloqueo()
    {
        if (saveHandler == null) return;

        // 1. Estrellas ganadas JUGANDO el nivel 1
        int starsEarned = saveHandler.LoadLevelStars(1);

        // 2. Estrellas COMPRADAS en la tienda
        int starsBought = saveHandler.GetBoughtStars();

        // 3. SUMA TOTAL
        int totalStars = starsEarned + starsBought;

        Debug.Log($"Check Desbloqueo: Nivel1({starsEarned}) + Tienda({starsBought}) = {totalStars}");

        // 4. LÓGICA DE DESBLOQUEO (Necesitas 3 o más en total)
        bool isUnlocked = totalStars >= 3;

        if (levelTwoButton != null)
        {
            levelTwoButton.interactable = isUnlocked;

            // Si tienes una imagen de candado, la ocultamos si está desbloqueado
            if (candadoNivel2 != null)
            {
                candadoNivel2.SetActive(!isUnlocked);
            }
        }
    }

    public void LevelOne()
    {
        IntentarCargarNivel(1);
    }

    public void LevelTwo()
    {
        // Doble verificación al hacer clic
        if (levelTwoButton.interactable)
        {
            IntentarCargarNivel(2);
        }
    }

    private void IntentarCargarNivel(int index)
    {
        StaminaSystem stamina = FindObjectOfType<StaminaSystem>();

        if (stamina != null)
        {
            // Verificamos si tiene suficiente energía
            if (stamina.currentStamina >= COSTO_ESTAMINA)
            {
                // SI TIENE: Usamos la energía y cargamos
                stamina.UseStamina(COSTO_ESTAMINA);
                CargarEscena(index);
            }
            else
            {
                // NO TIENE: Mostramos el aviso de error
                if (warningSystem != null) warningSystem.MostrarAviso();
                Debug.Log("No tenés suficiente Stamina para jugar.");
            }
        }
    }

    private void CargarEscena(int index)
    {
        if (sceneLoadManager != null)
            sceneLoadManager.SceneLoad(index);
        else
            SceneManager.LoadScene(index);
    }
}