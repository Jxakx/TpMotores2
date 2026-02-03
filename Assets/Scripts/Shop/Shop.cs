using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    [Header("--- UI ---")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI starsText;

    [Header("--- Botones ---")]
    [SerializeField] private Button btnDash;
    [SerializeField] private Button btnItem2; // Estrella x1
    [SerializeField] private Button btnItem3; // Estrella x2

    [Header("--- Precios ---")]
    [SerializeField] private int precioDash = 10;
    [SerializeField] private int precioItem2 = 5;
    [SerializeField] private int precioItem3 = 50;

    private JSONSaveHandler saveHandler;
    private int currentCoins;
    private int currentStarsBought;

    private void Start()
    {
        saveHandler = FindObjectOfType<JSONSaveHandler>();

        if (saveHandler != null)
        {
            // Cargamos datos al inicio usando los nombres correctos del Paso 1
            currentCoins = saveHandler.GetCoins();
            currentStarsBought = saveHandler.GetBoughtStars();

            // Check del dash
            if (saveHandler.LoadDashState())
            {
                BloquearBoton(btnDash);
            }
        }
        UpdateUI();
    }

    public void ComprarDash()
    {
        if (saveHandler == null) return;
        if (saveHandler.LoadDashState()) return;

        if (currentCoins >= precioDash)
        {
            currentCoins -= precioDash;
            saveHandler.SavePlayerData(currentCoins, currentStarsBought);
            saveHandler.SaveDashState(true);

            FindObjectOfType<Player>()?.UnlockDash();
            UpdateUI();
            BloquearBoton(btnDash);
        }
    }

    public void ComprarItem2() // 1 Estrella
    {
        if (currentCoins >= precioItem2)
        {
            currentCoins -= precioItem2;
            currentStarsBought += 1;

            // Guardamos monedas nuevas y estrellas nuevas
            saveHandler.SavePlayerData(currentCoins, currentStarsBought);

            UpdateUI();
            Debug.Log("Estrella comprada. Total compradas: " + currentStarsBought);
        }
    }

    public void ComprarItem3() // 2 Estrellas
    {
        if (currentCoins >= precioItem3)
        {
            currentCoins -= precioItem3;
            currentStarsBought += 2;

            saveHandler.SavePlayerData(currentCoins, currentStarsBought);

            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (coinsText != null) coinsText.text = "Dinero: " + currentCoins;

        if (starsText != null && saveHandler != null)
        {
            // OJO ACÁ: Sumamos las del Nivel 1 + las Compradas para mostrar el total
            int starsLevel1 = saveHandler.LoadLevelStars(1);
            int total = starsLevel1 + currentStarsBought;
            starsText.text = "Estrellas Totales: " + total;
        }
    }

    private void BloquearBoton(Button boton)
    {
        if (boton != null) boton.interactable = false;
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}