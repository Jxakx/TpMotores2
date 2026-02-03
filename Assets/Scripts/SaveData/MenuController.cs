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
        if (saveHandler == null) saveHandler = FindObjectOfType<JSONSaveHandler>();

        if (saveHandler != null)
        {
            // Lógica de desbloqueo SUMADA
            int s1 = saveHandler.LoadLevelStars(1);
            int bought = saveHandler.GetBoughtStars();
            int total = s1 + bought;

            if (buttonLevel2 != null)
                buttonLevel2.interactable = (total >= 3);

            if (starsText != null)
                starsText.text = total.ToString();
        }
    }
}