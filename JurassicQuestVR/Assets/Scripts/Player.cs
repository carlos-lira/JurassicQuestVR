using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public LevelManager levelManager;
    float playerHealth = 10f;
 
    public void PlayerHit(float damageTaken) 
    {
        playerHealth -= damageTaken;
        Debug.LogWarning("Player Health: " +playerHealth);

        if (playerHealth <= 0)
        {
            levelManager.EndGame(false);
        }
    }
}
