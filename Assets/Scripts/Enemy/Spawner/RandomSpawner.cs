using UnityEngine;
using UnityEngine.UI;

public class RandomSpawner : MonoBehaviour
{
    public static RandomSpawner Instance { get; private set; } // Singleton instance'ı
    public GameObject enemy; // Üreteceğimiz düşman prefab'i
    public Text enemyCount; // Düşman sayısını gösteren UI text'i
    public Transform destinationFolder; // Hedef dosyanın Transform'u
    [HideInInspector] public int spawnCount = 8; // Toplam üretilecek düşman sayısı
    private int currentSpawnCount = 0; // Şu ana kadar üretilen düşman sayısı

    // Singleton
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

    private void Update()
    {
        // Eğer üretilecek düşman varsa
        if (currentSpawnCount < spawnCount)
        {
            // Rastgele verilen aralık içerisinde bir üretim konumu belirler
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-15, 15), 0.05f, Random.Range(-15, 15));

            // Düşmanı üretir
            GameObject spawnedEnemy = Instantiate(enemy, randomSpawnPosition, Quaternion.identity);

            // // Üretilen düşmanı hedef dosyasının altına yerleştirir
            spawnedEnemy.transform.SetParent(destinationFolder);

            // Üretilen düşman sayısını günceller
            currentSpawnCount++;
        }
    }

    public void UpdateEnemyCount()
    {
        spawnCount--; // Üretilen düşman sayısını azaltır
        enemyCount.text = spawnCount.ToString(); // Düşman sayısını gösteren UI text'ini günceller
    }
}
