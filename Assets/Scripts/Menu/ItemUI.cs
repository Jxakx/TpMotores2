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

    public void InitializeButton (string nombre, Sprite icono, string costo)
    {
        textoNombre.text = nombre;
    }
}
