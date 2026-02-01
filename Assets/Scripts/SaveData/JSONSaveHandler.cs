using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONSaveHandler : MonoBehaviour
{
    private string filePath;
    private string savePath;
    private const string DashKey = "DashUnlocked";

    // Clase para datos del jugador
    [System.Serializable]
    public class PlayerData
    {
        public int coins;
        public int starsBought; // Estrellas compradas
    }

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/playerData.json";
    }

    void Start()
    {
        savePath = Application.persistentDataPath + "/level_data.json";
        Debug.Log($"Ruta de datos del jugador: {filePath}");
        Debug.Log($"Ruta de datos de estrellas: {savePath}");
    }

    // --- GUARDADO PRINCIPAL ---
    public void SavePlayerData(int coins, int starsBought)
    {
        PlayerData data = new PlayerData
        {
            coins = coins,
            starsBought = starsBought
        };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }

    // --- CARGA PRINCIPAL ---
    public PlayerData LoadPlayerData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            return new PlayerData { coins = 0, starsBought = 0 };
        }
    }

    // --- MÉTODOS DE AYUDA (GETTERS/SETTERS) ---

    // Este lo usa el Shop para saber cuánta plata tenés
    public int GetCoins()
    {
        return LoadPlayerData().coins;
    }

    // Este lo usa el Shop para saber cuántas estrellas compraste
    public int GetBoughtStars()
    {
        return LoadPlayerData().starsBought;
    }

    // Este guarda SOLO las estrellas compradas (sin borrar las monedas)
    public void SaveBoughtStars(int totalStars)
    {
        // 1. Cargamos lo que ya existe para no perder las monedas
        PlayerData currentData = LoadPlayerData();

        // 2. Modificamos solo las estrellas
        currentData.starsBought = totalStars;

        // 3. Guardamos todo junto de nuevo
        SavePlayerData(currentData.coins, currentData.starsBought);
    }

    // --- COMPATIBILIDAD (Para que no se rompan otros scripts viejos) ---

    public void SaveData(int coins)
    {
        PlayerData currentData = LoadPlayerData();
        currentData.coins = coins;
        SavePlayerData(currentData.coins, currentData.starsBought);
    }

    public int LoadData()
    {
        return LoadPlayerData().coins;
    }

    public int LoadStarsBought()
    {
        return LoadPlayerData().starsBought;
    }

    // --- GESTIÓN DEL DASH (PLAYERPREFS) ---

    public void SaveDashState(bool isUnlocked)
    {
        PlayerPrefs.SetInt(DashKey, isUnlocked ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool LoadDashState()
    {
        return PlayerPrefs.GetInt(DashKey, 0) == 1;
    }

    // --- GESTIÓN DE NIVELES (LEVEL DATA) ---

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

    public int GetTotalStars()
    {
        Dictionary<int, int> levelStars = LoadAllStars();
        int total = 0;
        foreach (var stars in levelStars.Values)
        {
            total += stars;
        }
        // Sumar las estrellas compradas
        total += GetBoughtStars();
        return total;
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

    // --- CLASES INTERNAS ---

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

    public void DeleteData()
    {
        if (File.Exists(filePath)) File.Delete(filePath);
        if (File.Exists(savePath)) File.Delete(savePath);
        if (PlayerPrefs.HasKey(DashKey)) PlayerPrefs.DeleteKey(DashKey);
        PlayerPrefs.Save();
        Debug.Log("Datos eliminados.");
    }
}