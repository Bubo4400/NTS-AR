using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Transform camTransform;
    public int EnemyNumber = 10;
    public float SpawnRange = 3f;
    public Material[] EnemyMaterials; // Array of materials to choose from

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < EnemyNumber; i++)
            SpawnEnemy();

        StartCoroutine(SpawnLoop()); // Correct coroutine usage
    }

    void Update()
    {
        // Nothing here unless you need real-time logic.
    }

    IEnumerator SpawnLoop()
    {
        for (int i = 0; i < 42; i++)
        {
            yield return new WaitForSeconds(4.2f); // Wait 1 second before spawning
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        float x = camTransform.position.x + Random.Range(-SpawnRange, SpawnRange);
        float y = camTransform.position.y + Random.Range(-SpawnRange, SpawnRange);
        float z = camTransform.position.z + Random.Range(-SpawnRange, SpawnRange);
        Vector3 spawnPos = new Vector3(x, y, z);
            
        GameObject enemy = Instantiate(EnemyPrefab, spawnPos, Quaternion.identity);
        AssignRandomMaterial(enemy);
    }

    void AssignRandomMaterial(GameObject enemy)
    {
        if (EnemyMaterials.Length > 0)
        {
            Material randomMaterial = EnemyMaterials[Random.Range(0, EnemyMaterials.Length)];
            Renderer enemyRenderer = enemy.GetComponent<Renderer>();
            if (enemyRenderer != null)
            {
                enemyRenderer.material = randomMaterial;
            }
        }
    }
}