using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public GameObject gamePanel, losePanel, winPanel;
    [SerializeField] EventTrigger playAgainButton;
    [SerializeField] EventTrigger backMenuButton;
    void Start()
    {
        gamePanel.SetActive(true);
        losePanel.SetActive(false);
        winPanel.SetActive(false);
    }

    public void Onlose()
    {
        gamePanel.SetActive(false);
        losePanel.SetActive(true);
        winPanel.SetActive(false);
    }

    public void Onwin()
    {
        gamePanel.SetActive(false);
        losePanel.SetActive(false);
        winPanel.SetActive(true);
    }

    public void BackTomenu()
    {
        SceneManager.LoadScene(0);
    }
}
