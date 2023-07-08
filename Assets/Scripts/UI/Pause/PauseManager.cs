using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // Pause menüsü gameobject'i
    public GameObject pauseBtn; // Pause menüsü gameobject'i

    public void Pause()
    {
        Time.timeScale = 0f; // Oyun zamanını durdur
        pauseMenu.SetActive(true); // Pause menüsünü aç
        pauseBtn.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1f; // Oyun zamanını tekrar başlat
        pauseMenu.SetActive(false); // Pause menüsünü kapat
        pauseBtn.SetActive(true);
    }

    public void RestartGame() //Restart btn
    {
        SceneManager.LoadScene("Game"); // Sahneyi başlat
        Time.timeScale = 1; // Oyun zamanını tekrar başlat
    }

    public void QuitGame() //quit btn
    {
        Application.Quit();
    }
}
