using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONSaveHandler : MonoBehaviour
{

    private string filePath;
    private const string DashKey = "DashUnlocked";
    private void Awake()
    {
        filePath = Application.persistentDataPath + "/playerData.json";
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
}
