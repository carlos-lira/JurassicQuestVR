using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    Animator anim;
    public LayerMask viewMask;
    public bool rangedEnemy = true;
    public AudioClip attackSound;

    public float accuracy = 0.5f;
    public float damage = 1f;

    private float shootCooldown = 0.5f;
    private float biteCooldown = 1f;
    private float shootReady;
    private float biteReady;

    // Start is called before the first frame update
    /*
    void Awake()
    {
        shootReady = shootCooldown;
        biteReady = biteCooldown;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("isSpotted", true);
    }
    */
    void Start()
    {
        shootReady = shootCooldown;
        biteReady = biteCooldown;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("isSpotted", true);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        bool canSee = CanSeePlayer();
        bool inRange = (distance <= agent.stoppingDistance) ? true: false;

        if (!inRange || !canSee)
        {
            agent.SetDestination(target.position);
            agent.isStopped = false;
            anim.SetBool("isRunning", true);

        }
        else {
            //TARGET IN RANGE AND VISIBLE

            //if target is too close, force stop
            if (distance <= 4f)
                agent.velocity = Vector3.zero;

            agent.isStopped = true;
            anim.SetBool("isRunning", false);
            Attack();
        }

    }

    bool CanSeePlayer()
    {
        //Check if player in LOS
        if (!Physics.Linecast(transform.position, target.position, viewMask, QueryTriggerInteraction.Ignore))
        {
            //PLAYER SPOTTED
            return true;
        }
        return false;
    }

    void Attack()
    {
        if (rangedEnemy)
        {
            Shoot();
        }
        else
        {
            Bite();
        }
    }

    void Shoot()
    {
        if (shootReady <= 0)
        {
            anim.SetBool("isShooting", true);
            GetComponent<AudioSource>().PlayOneShot(attackSound);
            if (Random.Range(0f, 1f) <= accuracy)
            {
                Debug.Log("hit");
                target.GetComponent<Player>().PlayerHit(damage);
            }
            else
            {
                Debug.Log("miss");
            }
            shootReady = shootCooldown;
        }
        else 
        {
            anim.SetBool("isShooting", false);
            shootReady -= Time.deltaTime;
        }

    }

    void Bite()
    {
        if (biteReady <= 0)
        {
            //anim.SetBool("isShooting", true);
            GetComponent<AudioSource>().PlayOneShot(attackSound);
            if (Random.Range(0f, 1f) <= accuracy)
            {
                Debug.Log("hit");
                target.GetComponent<Player>().PlayerHit(damage);
            }
            else
            {
                Debug.Log("miss");
            }
            biteReady = biteCooldown;
        }
        else
        {
            biteReady -= Time.deltaTime;
        }

    }
}
