using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONSaveHandler : MonoBehaviour
{

    private string filePath;
    private string savePath;
    private const string DashKey = "DashUnlocked";
    private void Awake()
    {
        filePath = Application.persistentDataPath + "/playerData.json";
    }

    void Start()
    {
        savePath = Application.persistentDataPath + "/level_data.json";
    }
    public void SaveData(int coins)
    {
        SaveData data = new SaveData { coins = coins };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }

    public int LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data.coins;
        }
        else
        {
            return 0; // Devuelve 0 si no existe un archivo
        }
    }

    public void DeleteData()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath); // Eliminar el archivo de datos si existe
            Debug.Log("Datos eliminados correctamente.");
        }
        else
        {
            Debug.Log("No hay datos para eliminar.");
        }

        if (PlayerPrefs.HasKey(DashKey))
        {
            PlayerPrefs.DeleteKey(DashKey);
            Debug.Log("Estado del dash eliminado.");
        }

        PlayerPrefs.Save();
    }

    public void SaveDashState(bool isUnlocked)
    {
        PlayerPrefs.SetInt(DashKey, isUnlocked ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool LoadDashState()
    {
        return PlayerPrefs.GetInt(DashKey, 0) == 1;
    }

    public void SaveStars(int levelIndex, int stars)
    {
        Dictionary<int, int> levelStars = LoadAllStars();
        levelStars[levelIndex] = stars;
        string json = JsonUtility.ToJson(new LevelDataWrapper(levelStars));
        File.WriteAllText(savePath, json);
    }

    public int LoadStars(int levelIndex)
    {
        Dictionary<int, int> levelStars = LoadAllStars();
        return levelStars.ContainsKey(levelIndex) ? levelStars[levelIndex] : 0;
    }

    private Dictionary<int, int> LoadAllStars()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            LevelDataWrapper dataWrapper = JsonUtility.FromJson<LevelDataWrapper>(json);
            return dataWrapper.ToDictionary();
        }
        return new Dictionary<int, int>();
    }

    [System.Serializable]
    private class LevelDataWrapper
    {
        public List<int> levelIndices;
        public List<int> starCounts;

        public LevelDataWrapper(Dictionary<int, int> levelStars)
        {
            levelIndices = new List<int>(levelStars.Keys);
            starCounts = new List<int>(levelStars.Values);
        }

        public Dictionary<int, int> ToDictionary()
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            for (int i = 0; i < levelIndices.Count; i++)
            {
                dictionary[levelIndices[i]] = starCounts[i];
            }
            return dictionary;
        }
    }
}
