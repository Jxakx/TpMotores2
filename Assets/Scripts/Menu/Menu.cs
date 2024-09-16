using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menuPanel, credits;
    [SerializeField] EventTrigger prototypeButton;
    [SerializeField] EventTrigger creditsButton;
    void Start()
    {
        ShowMain();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }
    public void ShowMain()
    {
        menuPanel.SetActive(true);
        credits.SetActive(false);
    }

    public void ShowCredits()
    {
        menuPanel.SetActive(false);
        credits.SetActive(true);
    }
}
