using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLevels : MonoBehaviour
{
    private Player player;

    public void LevelOne()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
        player.ResetPlayerCollisions();
    }

    public void LevelTwo()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
        player.ResetPlayerCollisions();
    }
}
