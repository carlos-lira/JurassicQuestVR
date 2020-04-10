using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public LevelManager levelManager;
    // Start is called before the first frame update
    public float enemyHealth = 3f;
    public bool inCombat = false;

    private Animator anim;
    private NavMeshAgent agent;


    private void Start()
    {
        //new
        levelManager = FindObjectOfType<LevelManager>();
        anim = GetComponentInChildren<Animator>();
        agent = GetComponentInChildren<NavMeshAgent>();
    }

    public void EnemyHit(float damageTaken)
    {
        anim.SetBool("Hit", true);

        if (!inCombat) 
            EnterCombat();

        enemyHealth -= damageTaken;

        if (enemyHealth == 0)
        {
            levelManager.EnemyKilled();
            anim.SetBool("Dead", true);
            agent.speed = agent.speed / 2;
            Destroy(gameObject, 3f);
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
