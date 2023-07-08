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
        pauseMenu.SetActive(true); // Pause menüsünü açar
        pauseBtn.SetActive(false); // Pause butonu gizler
    }

    public void Resume() // Oyuna devam etme butonu
    {
        Time.timeScale = 1f; // Oyun zamanını tekrar başlatır
        pauseMenu.SetActive(false); // Pause menüsünü kapatır
        pauseBtn.SetActive(true); // Pause butonu açar
    }

    public void RestartGame() //Oyuna yeniden başlama butonu
    {
        SceneManager.LoadScene("Game"); // Sahneyi başlatır
        Time.timeScale = 1; // Oyun zamanını tekrar başlatır
    }

    public void QuitGame() //quit btn
    {
        Application.Quit(); 
    }
}
