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
    private float shootReady;


    void Start()
    {
        shootReady = 0;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        shootReady -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, target.position);
        bool canSee = CanSeePlayer();
        bool inRange = (distance <= agent.stoppingDistance) ? true: false;

        if (!inRange || !canSee)
        {
            agent.SetDestination(target.position);
            agent.isStopped = false;
            anim.SetBool("Run", true);

        }
        else {
            //TARGET IN RANGE AND VISIBLE
            FaceTarget(target);

            //if target is too close, force stop
            if (distance <= 4f)
                agent.velocity = Vector3.zero;

            agent.isStopped = true;
            anim.SetBool("Run", false);
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
        if (shootReady <= 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("AimIdle"))
        {
            anim.SetBool("Shoot", true);
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

    }

    void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;

        //Removing Y axis to avoid weird model positioning
        direction = new Vector3(direction.x, 0, direction.z);

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 30);
    }
}
