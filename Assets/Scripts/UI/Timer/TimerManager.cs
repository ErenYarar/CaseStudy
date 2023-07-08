using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    [SerializeField] Text timerText;
    private float totalTime = 74f; // Geriye sayılacak süre (saniye cinsinden)
    private float currentTime;

    private void Start()
    {
        currentTime = totalTime;
        UpdateTimerText();
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            // Geriye sayım tamamlandığında:
            SceneManager.LoadScene("Game"); // Sahneyi yeniden başlat
        }
    }

    private void UpdateTimerText()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
        timerText.text = string.Format("{0}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }
}
