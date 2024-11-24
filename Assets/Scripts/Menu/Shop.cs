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

    [SerializeField] ItemDTO[] myItems = new ItemDTO[0];
    void Start()
    {
        for(int i = 0; i < myItems.Length; i++)
        {
            var newItem = Instantiate(itemPrefab, shopParent);
            newItem.InitializeButton(myItems[i]);
        }
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }
}
