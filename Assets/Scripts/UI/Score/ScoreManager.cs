using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    [HideInInspector] public int score = 0;  // Skor değişkeni, diğer scriptlerden erişilebilir
    public Text scoreText;  // Skoru göstermek için kullanılacak Text nesnesi

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseScore()
    {
        score += 500;  // Skoru 500 puan artır
        scoreText.text = score.ToString();  // Skoru güncelleyerek Text'e yazdır
    }
}
