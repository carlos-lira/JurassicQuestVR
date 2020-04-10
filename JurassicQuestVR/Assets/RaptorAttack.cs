using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RaptorAttack : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    Animator anim;
    public LayerMask viewMask;

    public AudioClip attackSound;
    public AudioClip[] warningSounds;
    private bool warned = false;

    public float accuracy = 1f;
    public float damage = 1f;

    private float biteCooldown = 1f;
    private float biteReady;

    void Start()
    {
        biteReady = 0;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        biteReady -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, target.position);
        bool canSee = CanSeePlayer();
        bool inRange = (distance <= agent.stoppingDistance) ? true : false;

        if (!inRange || !canSee)
        {
            agent.SetDestination(target.position);

            anim.SetBool("Move", true);
            agent.isStopped = false;

            if (distance <= 100 && !warned)
            {
                warned = true;
                anim.SetBool("Growl", true);
                GetComponent<AudioSource>().PlayOneShot(warningSounds[Random.Range(0, warningSounds.Length)]);
            }



            //Evaluating range attack
            if (canSee && distance <= agent.stoppingDistance * 3)
                Attack();

        }
        else
        {
            //TARGET IN RANGE AND VISIBLE
            agent.isStopped = true;

            FaceTarget(target);

            Attack();
        }

        if (biteReady <= 0 && CompletedAttack())
            DamagePlayer();

    }

    bool CompletedAttack()
    {
        if (
            anim.GetAnimatorTransitionInfo(0).IsName("Rap|RunAtk2 -> Rap|IdleA")
            || anim.GetAnimatorTransitionInfo(0).IsName("Rap|IdleAtk1 -> Rap|IdleA")
            || anim.GetAnimatorTransitionInfo(0).IsName("Rap|IdleAtk2 -> Rap|IdleA")
            || anim.GetAnimatorTransitionInfo(0).IsName("Rap|IdleAtk3 -> Rap|IdleA")
            )
            return true;

        return false;
    }

    void DamagePlayer()
    {
        biteReady = biteCooldown;

        if (Random.Range(0f, 1f) <= accuracy)
        {
            Debug.Log("hit");
            target.GetComponent<Player>().PlayerHit(damage);
        }
        else
        {
            Debug.Log("miss");
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
        if (biteReady <= 0 && anim.GetBool("AttackReady"))
        {
            GetComponent<AudioSource>().PlayOneShot(attackSound);
            anim.SetBool("Attack", true);
            anim.SetInteger("AttackType", Random.Range(1, 3));
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
