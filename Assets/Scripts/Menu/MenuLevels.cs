using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLevels : MonoBehaviour
{
    private int starsLevelOne; // Almacena las estrellas del nivel 1
    private string saveFilePath;
    public Button levelTwoButton; // Botón del nivel 2
    private SceneLoadManager sceneLoadManager; // Referencia al SceneLoadManager

    private void Start()
    {
        saveFilePath = Application.persistentDataPath + "/level_data.json";
        sceneLoadManager = FindObjectOfType<SceneLoadManager>(); // Buscar el gestor de carga de escenas
        LoadStarData();

        // Desactivar el botón del Nivel 2 si no tiene suficientes estrellas del nivel 1
        if (levelTwoButton != null)
        {
            levelTwoButton.interactable = starsLevelOne >= 2; // Verifica si el jugador tiene al menos 2 estrellas
        }
    }

    private void LoadStarData()
    {
        if (System.IO.File.Exists(saveFilePath))
        {
            string json = System.IO.File.ReadAllText(saveFilePath);
            LevelData levelData = JsonUtility.FromJson<LevelData>(json);

            starsLevelOne = 0; // Inicializamos las estrellas del nivel 1
            for (int i = 0; i < levelData.levelIndices.Count; i++)
            {
                if (levelData.levelIndices[i] == 1) // Solo contamos las estrellas del nivel 1
                {
                    starsLevelOne = levelData.starCounts[i]; // Cargamos las estrellas del nivel 1
                    break; // Solo necesitamos las estrellas del primer nivel
                }
            }
        }
        else
        {
            starsLevelOne = 0; // Si no hay datos, iniciar con 0 estrellas
        }
    }

    public void LevelOne()
    {
        if (sceneLoadManager != null)
        {
            sceneLoadManager.SceneLoad(1);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void LevelTwo()
    {
        // Verifica si el jugador tiene al menos 2 estrellas en el primer nivel
        if (starsLevelOne >= 2)
        {
            if (sceneLoadManager != null)
            {
                sceneLoadManager.SceneLoad(2);
            }
            else
            {
                SceneManager.LoadScene(2);
            }
        }
        else
        {
            Debug.Log("No tienes suficientes estrellas para desbloquear el Nivel 2.");
        }
    }
}
