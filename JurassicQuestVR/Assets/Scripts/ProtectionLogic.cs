using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionLogic : LevelManager
{
    private class Wave 
    {
        public int numEnemies;
        public GameObject enemy;
        public float spawnTime;
    }

    private List<Wave> waveBuilder;
    private Wave[] waves;
    private int waveCounter = 1;

    public int startingEnemies = 3;
    public int numWaves = 10;
    public float timeBetweenWaves = 30f;
    private float waveCountdown;
    private int totalEnemies = 0;

    public Transform spawnPointsHolder;
    private Transform[] spawnPoints;
    public GameObject raptor;


    private void Awake()
    {

        int waveEnemies = startingEnemies;
        totalEnemies = 0;

        waveBuilder = new List<Wave>();

        for (int i = 0; i < numWaves; i++)
        {
            Wave wave = new Wave();
            wave.numEnemies = waveEnemies;
            wave.spawnTime = timeBetweenWaves * 0.75f;
            totalEnemies += waveEnemies;

            waveBuilder.Add(wave);

            //Every other wave we increase the enemy count
            if ((i) % 2 == 0)
                waveEnemies++;
        }

        waves = waveBuilder.ToArray();
    }

    // Start is called before the first frame update
    void Start()
    {
        waveCountdown = timeBetweenWaves;
        spawnPoints = new Transform[spawnPointsHolder.childCount];
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = spawnPointsHolder.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waveCountdown <= 0f)
        {
            //SPAWN
            if (waveCounter <= waves.Length)
            {
                StartCoroutine(SpawnWave(waves[waveCounter-1]));
                waveCountdown = timeBetweenWaves;
                waveCounter++;
            }
            else if (AllEnemiesDead())
                EndGame(true);
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    IEnumerator SpawnWave(Wave wave) 
    {
        for (int i = 0; i < wave.numEnemies; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(wave.spawnTime / wave.numEnemies);
        }
        yield return null;
    }

    void SpawnEnemy(GameObject enemy)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var rap = Instantiate(raptor, spawnPoint.position, spawnPoint.rotation);
        //rap.gameObject.SetActive(true);
    }

    private bool AllEnemiesDead()
    {
        if (deadEnemies == totalEnemies)
            return true;
        else
            return false;
    }
}
