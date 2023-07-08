using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    [SerializeField] Text timerText; // Zamanlayıcı metni için UI Text 
    private float totalTime = 74f; // Geriye sayılacak süre (saniye cinsinden)
    private float currentTime;  // Geçen süre

    private void Start()
    {
        currentTime = totalTime; // Başlangıçta geçen süreyi toplam süreye ayarlar
        UpdateTimerText();
    }

    private void Update()
    {
        if (currentTime > 0) // Süre sıfırdan büyükse
        {
            currentTime -= Time.deltaTime;  // Geçen süreyi azalt
            UpdateTimerText();
        }
        else
        {
            // Geriye sayım tamamlandığında:
            SceneManager.LoadScene("Game"); // Sahneyi yeniden başlatır
        }
    }

    private void UpdateTimerText()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime); // Geçen süreyi TimeSpan nesnesine dönüştürür
        timerText.text = string.Format("{0}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds); // Zamanlayıcı metnini formatla ve günceller
    }
}
