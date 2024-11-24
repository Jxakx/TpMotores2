using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    [SerializeField] EventTrigger backMenuButton;

    [SerializeField] ItemUI itemPrefab;
    [SerializeField] Transform shopParent;
    void Start()
    {
        
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }
}
