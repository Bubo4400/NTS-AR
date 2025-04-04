using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplicationManager : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Transform camTransform;
    public int EnemyNumber = 2;
	public int EnemyCurrent = 0;
    public float SpawnRange = 2000f;
    public Material[] EnemyMaterials;
    public Text timerText;
    public Text gameover;
    public Button replay;
    private float startTime;
    private bool isRunning = false;
    public float delay = 5f;
    private float elapsedTime = 0f;

    void Start()
    {
        SpawnEnemy();
		
        StartStopwatch();
        StartCoroutine(SpawnLoop());

        gameover.gameObject.SetActive(false);
        replay.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime = Time.time - startTime;
            UpdateTimerText();
        }
    }

    IEnumerator SpawnLoop()
    {
        while (isRunning)
        {

            yield return new WaitForSeconds(delay);
            SpawnEnemy();
            delay = Mathf.Max(1f, delay - 0.1f); // Empêcher delay de devenir négatif
        }
    }

    public float MinSpawnDistance = 1000f; 
    public void SpawnEnemy()
    {
        Vector3 spawnPos;
        do
        {
            float x = camTransform.position.x + Random.Range(-SpawnRange, SpawnRange);
            float y = camTransform.position.y + Random.Range(-SpawnRange, SpawnRange);
            float z = camTransform.position.z + Random.Range(-SpawnRange, SpawnRange);
            spawnPos = new Vector3(x, y, z);
        }
        while (Vector3.Distance(spawnPos, camTransform.position) < MinSpawnDistance);

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
   
    public void StartStop()
    {
        if (isRunning)
        {
            Stop();
        }
        else
        {
            StartStopwatch();
        }
    }

    public void ResetStopwatch()
    {
        elapsedTime = 0f;
        startTime = Time.time;
        UpdateTimerText();
    }

    void StartStopwatch()
    {
        startTime = Time.time;
        isRunning = true;
    }

    public void Stop()
    {
        isRunning = false;
    }
    
    void UpdateTimerText()
    {
        string minutes = ((int)elapsedTime / 60).ToString("00");
        string seconds = (elapsedTime % 60).ToString("00");
        timerText.text = minutes + ":" + seconds;
    }

    public void Gameover()
    {
        gameover.gameObject.SetActive(true);
        replay.gameObject.SetActive(true);
		string minutes = ((int)elapsedTime / 60).ToString("00");
        string seconds = (elapsedTime % 60).ToString("00");
		gameover.text = "Womp Womp you died and only survived for : "+ minutes +"m" + seconds + "s";	
        Stop();
    }
}
