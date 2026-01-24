using UnityEngine;
using UnityEngine.UI; // Para los botones
using TMPro;          // Para el texto de las monedas
using UnityEngine.SceneManagement; // Para volver al menú

public class Shop : MonoBehaviour
{
    [Header("--- UI GENERAL ---")]
    [SerializeField] private TextMeshProUGUI coinsText; // Arrastrá acá tu texto de $$$

    [Header("--- ITEM 1: DASH (Habilidad) ---")]
    [SerializeField] private Button btnDash;        // Arrastrá el botón del Dash
    [SerializeField] private int precioDash = 10;   // Poné el precio acá

    [Header("--- ITEM 2 (Ej: Vida Extra) ---")]
    [SerializeField] private Button btnItem2;       // Arrastrá el botón del Item 2
    [SerializeField] private int precioItem2 = 5;

    [Header("--- ITEM 3 (Ej: Otro PowerUp) ---")]
    [SerializeField] private Button btnItem3;       // Arrastrá el botón del Item 3
    [SerializeField] private int precioItem3 = 50;

    // Variables internas
    private JSONSaveHandler saveHandler;
    private int currentCoins;

    private void Start()
    {
        // 1. Buscamos el sistema de guardado
        saveHandler = FindObjectOfType<JSONSaveHandler>();

        // 2. Cargamos las monedas y actualizamos el texto
        currentCoins = saveHandler.LoadData();
        UpdateCoinsUI();

        // 3. VERIFICACIÓN INICIAL:
        // Si ya compramos el Dash antes, desactivamos el botón apenas entramos.
        if (saveHandler.LoadDashState())
        {
            BloquearBoton(btnDash);
        }
    }

    // ====================================================
    //              FUNCIONES DE COMPRA
    // (Conectar estas funciones en el OnClick de cada botón)
    // ====================================================

    public void ComprarDash()
    {
        // Seguridad: Si ya lo tengo, no hago nada (por si el botón quedó activo por error)
        if (saveHandler.LoadDashState()) return;

        // ¿Me alcanza la plata?
        if (currentCoins >= precioDash)
        {
            // 1. Cobrar
            currentCoins -= precioDash;

            // 2. Guardar monedas nuevas
            saveHandler.SaveData(currentCoins);

            // 3. Guardar que ya tengo el Dash
            saveHandler.SaveDashState(true);

            // 4. Avisarle al Player (si está en escena) que lo desbloquee
            FindObjectOfType<Player>()?.UnlockDash();

            // 5. Actualizar UI visual
            UpdateCoinsUI();
            BloquearBoton(btnDash); // Pone el botón gris

            Debug.Log(" ¡Dash Comprado y Guardado!");
        }
        else
        {
            Debug.Log(" No tenés suficiente plata para el Dash.");
        }
    }

    public void ComprarItem2()
    {
        if (currentCoins >= precioItem2)
        {
            currentCoins -= precioItem2;
            saveHandler.SaveData(currentCoins);
            UpdateCoinsUI();

            Debug.Log(" Item 2 Comprado");
            // ACÁ AGREGÁ LA LÓGICA DE QUÉ TE DA ESTE ITEM
            // Ej: FindObjectOfType<Player>().Curar(1);
        }
        else
        {
            Debug.Log(" No tenés plata para el Item 2.");
        }
    }

    public void ComprarItem3()
    {
        if (currentCoins >= precioItem3)
        {
            currentCoins -= precioItem3;
            saveHandler.SaveData(currentCoins);
            UpdateCoinsUI();

            Debug.Log(" Item 3 Comprado");
            // ACÁ AGREGÁ LA LÓGICA DE QUÉ TE DA ESTE ITEM
        }
        else
        {
            Debug.Log(" No tenés plata para el Item 3.");
        }
    }

    // ====================================================
    //              FUNCIONES AUXILIARES
    // ====================================================

    private void UpdateCoinsUI()
    {
        if (coinsText != null)
        {
            coinsText.text = "Dinero: " + currentCoins.ToString();
        }
    }

    private void BloquearBoton(Button boton)
    {
        if (boton != null)
        {
            // Esto apaga la interacción y Unity automáticamente aplica el "Disabled Color"
            boton.interactable = false;
        }
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("Menu"); // Asegurate que tu escena se llame "Menu"
    }
}