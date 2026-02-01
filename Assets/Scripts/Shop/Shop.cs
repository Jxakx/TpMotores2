using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("--- UI GENERAL ---")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI starsText;

    [Header("--- ITEM 1: DASH (Habilidad) ---")]
    [SerializeField] private Button btnDash;
    [SerializeField] private int precioDash = 10;

    [Header("--- ITEM 2 (Compra 1 Estrella) ---")]
    [SerializeField] private Button btnItem2;
    [SerializeField] private int precioItem2 = 5;

    [Header("--- ITEM 3 (Compra 2 Estrellas) ---")]
    [SerializeField] private Button btnItem3;
    [SerializeField] private int precioItem3 = 50;

    private JSONSaveHandler saveHandler;
    private int currentCoins;
    private int currentStarsBought;

    private void Start()
    {
        saveHandler = FindObjectOfType<JSONSaveHandler>();

        // Usamos los métodos arreglados
        currentCoins = saveHandler.GetCoins();
        currentStarsBought = saveHandler.GetBoughtStars();

        UpdateUI();

        if (saveHandler.LoadDashState())
        {
            BloquearBoton(btnDash);
        }
    }

    public void ComprarDash()
    {
        if (saveHandler.LoadDashState()) return;

        if (currentCoins >= precioDash)
        {
            currentCoins -= precioDash;

            // Guardar
            saveHandler.SavePlayerData(currentCoins, currentStarsBought);
            saveHandler.SaveDashState(true);

            FindObjectOfType<Player>()?.UnlockDash();

            UpdateUI();
            BloquearBoton(btnDash);
            Debug.Log("¡Dash Comprado!");
        }
        else
        {
            Debug.Log("No hay plata.");
        }
    }

    public void ComprarItem2() // 1 Estrella
    {
        if (currentCoins >= precioItem2)
        {
            currentCoins -= precioItem2;
            currentStarsBought += 1;

            // Guardamos usando la nueva función segura
            saveHandler.SaveBoughtStars(currentStarsBought);
            // Aseguramos que las monedas también se actualicen en el JSON
            saveHandler.SavePlayerData(currentCoins, currentStarsBought);

            UpdateUI();
            Debug.Log("¡Estrella comprada!");
        }
        else
        {
            Debug.Log("No hay plata.");
        }
    }

    public void ComprarItem3() // 2 Estrellas
    {
        if (currentCoins >= precioItem3)
        {
            currentCoins -= precioItem3;
            currentStarsBought += 2;

            saveHandler.SaveBoughtStars(currentStarsBought);
            saveHandler.SavePlayerData(currentCoins, currentStarsBought);

            UpdateUI();
            Debug.Log("¡2 Estrellas compradas!");
        }
        else
        {
            Debug.Log("No hay plata.");
        }
    }

    private void UpdateUI()
    {
        if (coinsText != null) coinsText.text = "Dinero: " + currentCoins;

        if (starsText != null)
        {
            // OJO: Acá sumamos las del nivel 1 + las compradas para mostrar el total
            int level1Stars = saveHandler.LoadStars(1);
            int total = level1Stars + currentStarsBought;
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