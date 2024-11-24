using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textoNombre;
    [SerializeField] Image imagenIcono;
    [SerializeField] TextMeshProUGUI textoCosto;

    public event Action<ItemDTO> onItemClicked;
    ItemDTO itemToRepresent;
    public void InitializeButton (ItemDTO item)
    {
        textoNombre.text = item.itemName;
        imagenIcono.sprite = item.itemIcon;
        textoCosto.text = "$" + item.itemCost.ToString();
        itemToRepresent = item;
    }

    public void OnClickItem()
    {
        onItemClicked?.Invoke(itemToRepresent);
    }
}
