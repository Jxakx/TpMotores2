using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textoNombre;
    [SerializeField] Image imagenIcono;
    [SerializeField] TextMeshProUGUI textoCosto;

    public void InitializeButton (ItemDTO item)
    {
        textoNombre.text = item.itemName;
        imagenIcono.sprite = item.itemIcon;
        textoCosto.text = "$" + item.itemCost.ToString();
    }
}
