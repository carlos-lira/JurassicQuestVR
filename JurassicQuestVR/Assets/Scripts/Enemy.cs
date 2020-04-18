using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public LevelManager levelManager;
    // Start is called before the first frame update
    public float health = 3f;
    [System.NonSerialized]
    public bool inCombat = false;
    [System.NonSerialized]
    public bool activateCombat = false;
    [System.NonSerialized]
    public bool proximityAlert = false;

    public LayerMask viewMask;

    public Animator anim;
    public NavMeshAgent agent;
    public GameObject player;

    public CombatSettings combatSettings;

    public virtual void Start()
    {
        //new
        levelManager = FindObjectOfType<LevelManager>();
        anim = GetComponentInChildren<Animator>();
        agent = GetComponentInChildren<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void EnemyHit(float damageTaken)
    {
        anim.SetBool("Hit", true);

        
        if (!inCombat) 
            activateCombat = true;
        
        health -= damageTaken;

        if (health == 0)
        {
            levelManager.EnemyKilled();
            anim.SetBool("Dead", true);
            agent.isStopped = true;
            Destroy(gameObject, 3f);
        }
    }

    public void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;

        //Removing Y axis to avoid weird model positioning
        direction = new Vector3(direction.x, 0, direction.z);

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 30);
    }

    public bool PlayerInLOS()
    {
        //Check if player in LOS
        if (!Physics.Linecast(transform.position, player.transform.position, viewMask, QueryTriggerInteraction.Ignore))
        {
            //PLAYER SPOTTED
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //PROXIMITY DETECT
        if (other.gameObject.tag == "Player")
            proximityAlert = true;
    }

    [System.Serializable]
    public class CombatSettings
    {
        public AudioClip attackSound;
        public float accuracy = 0.5f;
        public float damage = 1f;
        public float attackCooldown = 0.5f;
    }

}
