using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    public Button buttonLevel2;
    public JSONSaveHandler saveHandler;
    public TMP_Text starsText;

    void Start()
    {
        // Si saveHandler no está asignado en el inspector, intentamos encontrarlo
        if (saveHandler == null)
        {
            saveHandler = FindObjectOfType<JSONSaveHandler>();
        }

        if (saveHandler != null)
        {
            // Carga las estrellas del nivel 1 para habilitar/deshabilitar el botón
            int starsLevel1 = saveHandler.LoadStars(1);
            buttonLevel2.interactable = (starsLevel1 == 3);

            // Obtiene el total de estrellas (niveles + compradas)
            int totalStars = saveHandler.GetTotalStars();

            // Obtiene estrellas compradas para mostrarlas si lo deseas
            int boughtStars = saveHandler.LoadStarsBought();

            // Muestra el total de estrellas
            // Si quieres mostrar más detalles:
            // starsText.text = $"Estrellas: {totalStars} (Compradas: {boughtStars})";
            starsText.text = totalStars.ToString();

            Debug.Log($"MenuController: Estrellas totales = {totalStars}, Compradas = {boughtStars}");
        }
        else
        {
            Debug.LogError("JSONSaveHandler no encontrado!");
            starsText.text = "0";
        }
    }

    // Método para actualizar el UI cuando regresas de la tienda
    public void UpdateStarsDisplay()
    {
        if (saveHandler != null)
        {
            int totalStars = saveHandler.GetTotalStars();
            starsText.text = totalStars.ToString();
        }
    }
}