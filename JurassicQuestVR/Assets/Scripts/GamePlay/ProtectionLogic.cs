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


    public AudioClip lauraFirstDialogue;
    bool firstDialoguePlayed = false;
    public AudioClip raptorSound;
    bool raptorSoundPlayed = false;
    public AudioClip lauraSecondDialogue;
    bool secondDialoguePlayed = false;
    public AudioClip lauraThirdDialogue;
    bool thirdDialoguePlayed = false;
    public AudioClip lauraFourthDialogue;
    bool fourthDialoguePlayed = false;
    public AudioClip lauraFifthDialogue;
    bool fifthDialoguePlayed = false;

    Laura laura;
    float timer;
    public GameObject raptorSoundSource;

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
    public override void Start()
    {
        base.Start();
        timer = 0;
        laura = GameObject.FindGameObjectWithTag("Laura").GetComponent<Laura>();

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
        timer += Time.deltaTime;

        if (timer >= 5f && !firstDialoguePlayed)
        {
            firstDialoguePlayed = true;
            laura.PlaySound(lauraFirstDialogue);
        }

        if (timer >= 13f && !raptorSoundPlayed)
        {
            raptorSoundPlayed = true;
            raptorSoundSource.GetComponent<AudioSource>().PlayOneShot(raptorSound); ;
        }

        if (timer >= 15f && !secondDialoguePlayed)
        {
            secondDialoguePlayed = true;
            laura.PlaySound(lauraSecondDialogue);
        }

        if (deadEnemies >= totalEnemies / 2 && !thirdDialoguePlayed)
        {
            thirdDialoguePlayed = true;
            laura.PlaySound(lauraThirdDialogue);
        }

        if (deadEnemies >= totalEnemies * 3 / 4 && !fourthDialoguePlayed)
        {
            fourthDialoguePlayed = true;
            laura.PlaySound(lauraFourthDialogue);
        }

        if (waveCountdown <= 0f)
        {
            //SPAWN
            if (waveCounter <= waves.Length)
            {
                StartCoroutine(SpawnWave(waves[waveCounter - 1]));
                waveCountdown = timeBetweenWaves;
                waveCounter++;
            }
            else if (AllEnemiesDead())
            {
                if (!fifthDialoguePlayed)
                {
                    fifthDialoguePlayed = true;
                    laura.PlaySound(lauraFifthDialogue);
                }
                EndGame(true);
            }
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
