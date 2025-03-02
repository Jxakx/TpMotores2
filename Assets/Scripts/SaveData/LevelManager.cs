using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int levelIndex;
    private float levelStartTime;
    private JSONSaveHandler saveHandler;

    void Start()
    {
        levelStartTime = Time.time;
        saveHandler = FindObjectOfType<JSONSaveHandler>();
    }

    public void CompleteLevel()
    {
        float elapsedTime = Time.time - levelStartTime;
        int starsEarned = CalculateStars(elapsedTime);
        saveHandler.SaveStars(levelIndex, starsEarned);
    }

    int CalculateStars(float time)
    {
        if (time < 60f)
            return 3;
        else if (time < 90f)
            return 2;
        else
            return 1;
    }

    public int LoadStars()
    {
        return saveHandler.LoadStars(levelIndex);
    }
}
