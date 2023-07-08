using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] Text countdownText; // Geri sayım metni için UI Text bileşeni
    [SerializeField] Animator countdownAnimator; // Geri sayım animasyonu için Animator bileşeni
    [SerializeField] GameObject startPanel;  // Başlangıç GENEL paneli
    [SerializeField] GameObject pauseBTN; // Pause butonu (Başta basılamasın diye kapatılacak)

    void Start()
    {
        pauseBTN.SetActive(false); // Pause butonunu başlangıçta gizler
        StartCoroutine(CountdownCoroutine()); // Geri sayım işlemini başlatır
    }

    IEnumerator CountdownCoroutine()
    {
        Time.timeScale = 0f; // Oyun zamanını duraklatır (Arka planda bir şey oynamaması için)

        countdownAnimator.SetTrigger("Countdown"); // Geri sayım animasyonu başlat

        countdownText.text = "3"; 
        yield return new WaitForSecondsRealtime(1f); // 1 saniye bekle

        countdownText.text = "2";
        yield return new WaitForSecondsRealtime(1f); // 1 saniye bekle

        countdownText.text = "1";
        yield return new WaitForSecondsRealtime(1f); // 1 saniye bekle

        countdownText.text = "GO!!!!";

        yield return new WaitForSecondsRealtime(1f); // Animasyonun tamamlanmasını beklemek için // 1 saniye süre

        Time.timeScale = 1f; // Oyun zamanı geri başlatır
        startPanel.SetActive(false); // GENEL Paneli gizle
        pauseBTN.SetActive(true); // pause butonu oyun başlayınca getirir
    }
}
