using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    [Header("--- UI GENERAL ---")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI starsText; // Nuevo: para mostrar estrellas totales

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

        // Cargar datos del jugador
        currentCoins = saveHandler.LoadData();
        currentStarsBought = saveHandler.LoadStarsBought();

        UpdateUI();

        // Verificación inicial del Dash
        if (saveHandler.LoadDashState())
        {
            BloquearBoton(btnDash);
            BloquearBoton(btnItem2);
            BloquearBoton(btnItem3);
        }
    }

    public void ComprarDash()
    {
        if (saveHandler.LoadDashState()) return;

        if (currentCoins >= precioDash)
        {
            currentCoins -= precioDash;

            // Guardar usando el nuevo método
            saveHandler.SavePlayerData(currentCoins, currentStarsBought);
            saveHandler.SaveDashState(true);

            FindObjectOfType<Player>()?.UnlockDash();

            UpdateUI();
            BloquearBoton(btnDash);

            Debug.Log(" ¡Dash Comprado y Guardado!");
        }
        else
        {
            Debug.Log(" No tenés suficiente plata para el Dash.");
        }
    }

    public void ComprarItem2() // Compra 1 estrella
    {
        if (currentCoins >= precioItem2)
        {
            currentCoins -= precioItem2;
            currentStarsBought += 1; // Incrementar estrellas compradas

            // Guardar ambos valores
            saveHandler.SavePlayerData(currentCoins, currentStarsBought);

            UpdateUI();
            BloquearBoton(btnItem2);

            Debug.Log(" ¡1 Estrella comprada! Total estrellas compradas: " + currentStarsBought);
            Debug.Log(" Estrellas totales (niveles + compradas): " + saveHandler.GetTotalStars());
        }
        else
        {
            Debug.Log(" No tenés plata para comprar 1 estrella.");
        }
    }

    public void ComprarItem3() // Compra 2 estrellas
    {
        if (currentCoins >= precioItem3)
        {
            currentCoins -= precioItem3;
            currentStarsBought += 2; // Incrementar 2 estrellas compradas

            // Guardar ambos valores
            saveHandler.SavePlayerData(currentCoins, currentStarsBought);

            UpdateUI();
            BloquearBoton(btnItem3);

            Debug.Log(" ¡2 Estrellas compradas! Total estrellas compradas: " + currentStarsBought);
            Debug.Log(" Estrellas totales (niveles + compradas): " + saveHandler.GetTotalStars());
        }
        else
        {
            Debug.Log(" No tenés plata para comprar 2 estrellas.");
        }
    }

    private void UpdateUI()
    {
        // Actualizar monedas
        if (coinsText != null)
        {
            coinsText.text = "Dinero: " + currentCoins.ToString();
        }

        // Actualizar estrellas (opcional)
        if (starsText != null)
        {
            int totalStars = saveHandler.GetTotalStars();
            starsText.text = "Estrellas totales: " + totalStars +
                           " (Compradas: " + currentStarsBought + ")";
        }

        // Buscar y actualizar el MenuController si está en la escena
        MenuController menuController = FindObjectOfType<MenuController>();
        if (menuController != null)
        {
            menuController.UpdateStarsDisplay();
        }
    }

    private void BloquearBoton(Button boton)
    {
        if (boton != null)
        {
            boton.interactable = false;
        }
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}