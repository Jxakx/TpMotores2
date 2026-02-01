using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLevels : MonoBehaviour
{
    [Header("Referencias UI")]
    public Button levelTwoButton; // El botón del Nivel 2
    public GameObject candadoNivel2; // [ARRASTRAR ACÁ LA IMAGEN DEL CANDADO]

    private JSONSaveHandler saveHandler;
    private SceneLoadManager sceneLoadManager;
    private int starsLevelOne;
    private int starsBought;

    private void Start()
    {
        saveHandler = FindObjectOfType<JSONSaveHandler>();
        sceneLoadManager = FindObjectOfType<SceneLoadManager>();

        if (saveHandler != null)
        {
            // 1. Cargamos estrellas ganadas jugando (Nivel 1)
            starsLevelOne = saveHandler.LoadStars(1);

            // 2. Cargamos estrellas compradas en tienda
            starsBought = saveHandler.GetBoughtStars();
        }

        // 3. Calculamos el TOTAL REAL
        int totalStars = starsLevelOne + starsBought;

        Debug.Log($"Estrellas Nivel 1: {starsLevelOne} | Compradas: {starsBought} | Total: {totalStars}");

        // 4. Lógica de desbloqueo (Necesita 3 o más en total)
        if (totalStars >= 3)
        {
            // Desbloqueado
            if (levelTwoButton != null) levelTwoButton.interactable = true;
            if (candadoNivel2 != null) candadoNivel2.SetActive(false);
        }
        else
        {
            // Bloqueado
            if (levelTwoButton != null) levelTwoButton.interactable = false;
            if (candadoNivel2 != null) candadoNivel2.SetActive(true);
        }
    }

    public void LevelOne()
    {
        if (sceneLoadManager != null)
            sceneLoadManager.SceneLoad(1);
        else
            SceneManager.LoadScene(1); // Asegurate que el nivel 1 esté en Build Settings
    }

    public void LevelTwo()
    {
        // Doble chequeo de seguridad
        int total = starsLevelOne + starsBought;

        if (total >= 3)
        {
            if (sceneLoadManager != null)
                sceneLoadManager.SceneLoad(2); // Asegurate que el nivel 2 es el índice 2 o el nombre correcto
            else
                SceneManager.LoadScene(2);
        }
        else
        {
            Debug.Log("Aún no tienes 3 estrellas en total.");
        }
    }
}