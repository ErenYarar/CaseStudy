using UnityEngine;
using UnityEngine.UI;

public class RandomSpawner : MonoBehaviour
{
    public static RandomSpawner Instance { get; private set; }
    public GameObject enemy;
    public Text enemyCount;
    public Transform destinationFolder; // Hedef dosyanın Transformu
    [HideInInspector] public int spawnCount = 8;
    private int currentSpawnCount = 0;

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
        if (currentSpawnCount < spawnCount)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-15, 15), 0.05f, Random.Range(-15, 15));
            GameObject spawnedEnemy = Instantiate(enemy, randomSpawnPosition, Quaternion.identity);
            // Klon hedef dosyası
            spawnedEnemy.transform.SetParent(destinationFolder);
            currentSpawnCount++;
        }
    }

    public void UpdateEnemyCount()
    {
        spawnCount--;
        enemyCount.text = spawnCount.ToString();
    }
}
