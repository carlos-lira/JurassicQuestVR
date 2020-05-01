using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raptor : Enemy
{

    private float attackReady = 0f;

    public AudioClip[] warningSounds;
    private bool warned = false;
    private bool dealtDamage = false;

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            attackReady -= Time.deltaTime;
            attackReady = Mathf.Clamp(attackReady, 0, combatSettings.attackCooldown);

            float distance = Vector3.Distance(transform.position, player.transform.position);
            bool canSee = PlayerInLOS();
            bool inRange = (distance <= agent.stoppingDistance) ? true : false;

            if (!inRange || !canSee)
            {
                agent.SetDestination(player.transform.position);

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

                FaceTarget(player.transform);

                Attack();
            }

            if (CompletedAttack() && !dealtDamage)
            {
                dealtDamage = true;
                DamagePlayer();
            }
        }
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

    bool ReadyToAtack()
    {
        if (
        anim.GetCurrentAnimatorStateInfo(0).IsName("Rap|IdleA")
        || anim.GetCurrentAnimatorStateInfo(0).IsName("Rap|Run")
        || anim.GetCurrentAnimatorStateInfo(0).IsName("Rap|Walk")
        )
            return true;

        return false;
    }
    void DamagePlayer()
    {
        if (Random.Range(0f, 1f) <= combatSettings.accuracy)
        {
            Debug.Log("hit");
            player.GetComponent<Player>().PlayerHit(combatSettings.damage);
        }
        else
        {
            Debug.Log("miss");
        }
    }

    void Attack()
    {
        if (attackReady <= 0 && ReadyToAtack())
        {
            dealtDamage = false;
            attackReady = combatSettings.attackCooldown;
            GetComponent<AudioSource>().PlayOneShot(combatSettings.attackSound);
            anim.SetBool("Attack", true);
            anim.SetInteger("AttackType", Random.Range(1, 3));
        }
    }
}
