using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timeElapsed = 0f;
    private float timeLimit = 300f;

    void Update()
    {
        if (timeElapsed < timeLimit)
        {
            timeElapsed += Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            timeElapsed = timeLimit;
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);

        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }
}