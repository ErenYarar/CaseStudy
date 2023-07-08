using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [SerializeField] Text timerText;
    private float totalTime = 74f; // Geriye sayılacak süre (saniye cinsinden)
    private float currentTime;

    private void Start()
    {
        currentTime = totalTime;
        UpdateTimerText();
        StartTimer();
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
            // Geriye sayım tamamlandığında yapılmasını istediğiniz işlemleri burada gerçekleştirebilirsiniz.
            // Örneğin, oyunu durdurabilir veya bir işlevi çağırabilirsiniz.
        }
    }

    private void UpdateTimerText()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
        timerText.text = string.Format("{0}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    private void StartTimer()
    {
        // Timer'ı başlatmak için gereken kodları buraya yazabilirsiniz.
        // Örneğin, gerekirse bir animasyonu başlatabilir veya diğer oyun nesnelerine komut verebilirsiniz.
    }
}
