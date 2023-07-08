using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] Text countdownText;
    [SerializeField] Animator countdownAnimator;
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject pauseBTN;

    void Start()
    {
        pauseBTN.SetActive(false);
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        Time.timeScale = 0f;

        countdownAnimator.SetTrigger("Countdown"); // Animasyonu tetikle

        countdownText.text = "3";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "2";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "1";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "GO!!!!";

        yield return new WaitForSecondsRealtime(1f); // Animasyonun tamamlanmasını beklemek için süre ekle

        Time.timeScale = 1f; // Oyun zamanı geri başlat
        startPanel.SetActive(false); // Paneli gizle
        pauseBTN.SetActive(true); // pause butonu oyun başlayınca getir
    }
}
