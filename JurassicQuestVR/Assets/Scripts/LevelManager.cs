using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameManager gameManager;
    public int deadEnemies;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        deadEnemies = 0;
    }

    public void EnemyKilled()
    {
        deadEnemies++;
    }

    public void EndGame(bool victory)
    {
        if (victory)
        {
            Debug.LogWarning("YOU WON!");
        }
        else
        {
            Debug.Log("YOU LOST!");
        }
    }
}

