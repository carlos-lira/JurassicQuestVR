using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public LevelManager levelManager;
    // Start is called before the first frame update
    public float enemyHealth = 3f;
    public bool inCombat = false;


    private void Start()
    {
        //new
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void EnemyHit(float damageTaken)
    {
        if (!inCombat) 
            EnterCombat();

        enemyHealth -= damageTaken;

        if (enemyHealth == 0)
        {
            levelManager.EnemyKilled();
            Destroy(gameObject, 0.5f);
        }
    }

    public void EnterCombat()
    {
        inCombat = true;
        if (gameObject.GetComponent<SoldierPatrol>() != null)
        {
            if (gameObject.GetComponent<SoldierPatrol>().enabled == true)
                gameObject.GetComponent<SoldierPatrol>().EnterCombat();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //PROXIMITY DETECT
        if (other.gameObject.tag == "Player")
            EnterCombat();
    }

}
