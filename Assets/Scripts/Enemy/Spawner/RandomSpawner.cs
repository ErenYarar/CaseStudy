using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject enemy;
    public Transform destinationFolder; // Hedef dosyanın Transformu

    public int spawnCount = 20;
    private int currentSpawnCount = 0;

    private void Update()
    {
        if (currentSpawnCount < spawnCount)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-20, 20), 0.05f, Random.Range(-20, 20));
            GameObject spawnedEnemy = Instantiate(enemy, randomSpawnPosition, Quaternion.identity);
            
            // Klon hedef dosyası
            spawnedEnemy.transform.SetParent(destinationFolder);
            
            currentSpawnCount++;
        }
    }
}
