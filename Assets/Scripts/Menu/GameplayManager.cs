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
    [SerializeField] EventTrigger CheckPointButton;
    private Player player;
    void Start()
    {
        gamePanel.SetActive(true);
        losePanel.SetActive(false);
        winPanel.SetActive(false);

        player = FindObjectOfType<Player>();
    }

    public void Onlose()
    {
        gamePanel.SetActive(false);
        losePanel.SetActive(true);
        winPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Onwin()
    {
        gamePanel.SetActive(false);
        losePanel.SetActive(false);
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
        player.ResetPlayerCollisions();
    }
    public void BackTomenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RespawnAtCheckpoint()
    {
        Time.timeScale = 1f;
        losePanel.SetActive(false);
        gamePanel.SetActive(true);
        player.RespawnAtCheckpoint(); // Llama a la función de respawn en el script del jugador
    }
}
