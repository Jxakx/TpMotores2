using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLevels : MonoBehaviour
{
    private int totalStars; // Para almacenar las estrellas guardadas
    private string saveFilePath;
    public Button levelTwoButton; // Botón del nivel 2
    private SceneLoadManager sceneLoadManager; // Referencia al SceneLoadManager

    private void Start()
    {
        saveFilePath = Application.persistentDataPath + "/level_data.json";
        sceneLoadManager = FindObjectOfType<SceneLoadManager>(); // Buscar el gestor de carga de escenas
        LoadStarData();

        // Desactivar el botón del Nivel 2 si no tiene suficientes estrellas
        if (levelTwoButton != null)
        {
            levelTwoButton.interactable = totalStars >= 2;
        }
    }

    private void LoadStarData()
    {
        if (System.IO.File.Exists(saveFilePath))
        {
            string json = System.IO.File.ReadAllText(saveFilePath);
            LevelData levelData = JsonUtility.FromJson<LevelData>(json);

            totalStars = 0;
            foreach (int stars in levelData.starCounts)
            {
                totalStars += stars; // Sumar todas las estrellas obtenidas
            }
        }
        else
        {
            totalStars = 0; // Si no hay datos, iniciar con 0 estrellas
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
        if (totalStars >= 2)
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
