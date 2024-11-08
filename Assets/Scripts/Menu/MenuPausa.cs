using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPausa : MonoBehaviour
{
    public void Pause()
    {
        Time.timeScale = 0f;
    }
}
