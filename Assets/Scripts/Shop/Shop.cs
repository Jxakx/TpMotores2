using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    [Header("--- UI ---")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI starsText;
    [SerializeField] private UIWarning warningSinPlata; // <-- NUEVO: Acá va tu cartel rojo

    [Header("--- Botones ---")]
    [SerializeField] private Button btnDash;
    [SerializeField] private Button btnItem2;
    [SerializeField] private Button btnItem3;
    [SerializeField] private Button btnEnergia;

    [Header("--- Precios ---")]
    [SerializeField] private int precioDash = 10;
    [SerializeField] private int precioItem2 = 5;
    [SerializeField] private int precioItem3 = 50;
    [SerializeField] private int precioEnergia = 20;

    private JSONSaveHandler saveHandler;
    private int currentCoins;
    private int currentStarsBought;

    private void Start()
    {
        saveHandler = FindObjectOfType<JSONSaveHandler>();

        if (saveHandler != null)
        {
            currentCoins = saveHandler.GetCoins();
            currentStarsBought = saveHandler.GetBoughtStars();

            if (saveHandler.LoadDashState())
            {
                BloquearBoton(btnDash);
            }
        }
        UpdateUI();
    }

    // --- COMPRA DE DASH ---
    public void ComprarDash()
    {
        if (saveHandler == null) return;
        if (saveHandler.LoadDashState()) return;

        if (currentCoins >= precioDash)
        {
            ConfirmPopup.Instance.MostrarPopup(EjecutarCompraDash);
        }
        else
        {
            MostrarErrorDinero(); // Llama al cartel rojo
        }
    }

    private void EjecutarCompraDash()
    {
        currentCoins -= precioDash;
        saveHandler.SavePlayerData(currentCoins, currentStarsBought);
        saveHandler.SaveDashState(true);

        FindObjectOfType<Player>()?.UnlockDash();
        UpdateUI();
        BloquearBoton(btnDash);
    }

    // --- COMPRA DE ENERGÍA ---
    public void ComprarEnergia()
    {
        if (saveHandler == null) return;

        if (currentCoins >= precioEnergia)
        {
            ConfirmPopup.Instance.MostrarPopup(EjecutarCompraEnergia);
        }
        else
        {
            MostrarErrorDinero(); // Llama al cartel rojo
        }
    }

    private void EjecutarCompraEnergia()
    {
        StaminaSystem staminaSystem = FindObjectOfType<StaminaSystem>();

        if (staminaSystem != null)
        {
            currentCoins -= precioEnergia;
            saveHandler.SavePlayerData(currentCoins, currentStarsBought);

            staminaSystem.RechargeStamina(5);

            UpdateUI();
        }
    }

    // --- COMPRA 1 ESTRELLA ---
    public void ComprarItem2()
    {
        if (currentCoins >= precioItem2)
        {
            ConfirmPopup.Instance.MostrarPopup(EjecutarCompraItem2);
        }
        else
        {
            MostrarErrorDinero(); // Llama al cartel rojo
        }
    }

    private void EjecutarCompraItem2()
    {
        currentCoins -= precioItem2;
        currentStarsBought += 1;

        saveHandler.SavePlayerData(currentCoins, currentStarsBought);
        UpdateUI();
    }

    // --- COMPRA 2 ESTRELLAS ---
    public void ComprarItem3()
    {
        if (currentCoins >= precioItem3)
        {
            ConfirmPopup.Instance.MostrarPopup(EjecutarCompraItem3);
        }
        else
        {
            MostrarErrorDinero(); // Llama al cartel rojo
        }
    }

    private void EjecutarCompraItem3()
    {
        currentCoins -= precioItem3;
        currentStarsBought += 2;

        saveHandler.SavePlayerData(currentCoins, currentStarsBought);
        UpdateUI();
    }

    // --- NUEVA FUNCIÓN: MOSTRAR CARTEL ROJO ---
    private void MostrarErrorDinero()
    {
        if (warningSinPlata != null)
        {
            warningSinPlata.MostrarAviso();
        }
        else
        {
            Debug.LogWarning("Ojo: Te olvidaste de arrastrar el cartel rojo al Inspector de la Tienda.");
        }
    }

    // --- ACTUALIZACIÓN VISUAL ---
    private void UpdateUI()
    {
        if (coinsText != null) coinsText.text = "" + currentCoins;

        if (starsText != null && saveHandler != null)
        {
            int starsLevel1 = saveHandler.LoadLevelStars(1);
            int total = starsLevel1 + currentStarsBought;
            starsText.text = "Estrellas Totales: " + total;
        }
    }

    private void BloquearBoton(Button boton)
    {
        if (boton != null)
        {
            boton.interactable = false;
        }
    }

    // --- BOTÓN BACK ---
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}