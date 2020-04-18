using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Enemy
{
    // Start is called before the first frame update
    [SerializeField]
    PatrolSettings patrolSettings;

    [SerializeField]
    SoldierSettings soldierSettings;

    private Transform playerPosition;
    private float timeSpotted = 0f;
    private bool combatEnabled;

    private float shootReady = 0f;

    public override void Start()
    {
        base.Start();
        playerPosition = player.transform;
        ApplySpotlightSettings();

        combatEnabled = levelManager.combatEnabled;

        if (patrolSettings.soldierPath == null)
            patrolSettings.isStationary = true;

        if (!patrolSettings.isStationary)
        {
            Vector3[] waypoints = new Vector3[patrolSettings.soldierPath.childCount];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = patrolSettings.soldierPath.GetChild(i).position;
            }

            StartCoroutine(FollowPath(waypoints));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activateCombat && !inCombat)
            EnterCombat();

        if (health > 0 && !levelManager.IsGameOver())
        {
            if (!inCombat)
            {
                //Patrolling state
                CheckForPlayer();

                //If player is too close, we enter combat
                if (proximityAlert)
                    EnterCombat();
            }
            else
            {
                FaceTarget(player.transform);
                //Combat state
                if (!combatEnabled)
                {
                    levelManager.EndGame(false);
                }
                else
                {
                    FightPlayer();
                }
            }
        }
    }



    public void EnterCombat() 
    {
        if (!inCombat)
        {
            inCombat = true;
            soldierSettings.spotLight.color = Color.red;
            anim.SetBool("InCombat", true);
            StopAllCoroutines();
        }
    }

    void CheckForPlayer()
    {
        if (timeSpotted < patrolSettings.timeToSpotPlayer)
        {
            if (CanSeePlayer())
            {
                //Player Spotted
                anim.SetBool("PlayerSpotted", true);
                soldierSettings.spotLight.color = Color.yellow;
                timeSpotted += Time.deltaTime;
            }
            else
            {
                //Back to patrolling
                anim.SetBool("PlayerSpotted", false);
                timeSpotted -= Time.deltaTime;
            }
        }
        else
        {
            soldierSettings.spotLight.color = Color.red;
            EnterCombat();
        }

        timeSpotted = Mathf.Clamp(timeSpotted, 0, patrolSettings.timeToSpotPlayer);

        if (timeSpotted == 0)
            soldierSettings.spotLight.color = soldierSettings.originalSpotLightColor;
    }

    void FightPlayer()
    {
        shootReady -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        bool playerInLOS = PlayerInLOS();
        bool inRange = (distance <= agent.stoppingDistance) ? true : false;

        if (!inRange || !playerInLOS)
        {
            agent.SetDestination(player.transform.position);
            agent.isStopped = false;
            anim.SetBool("Run", true);

        }
        else
        {
            //TARGET IN RANGE AND VISIBLE
            FaceTarget(player.transform);

            //if target is too close, force stop
            if (distance <= 4f)
                agent.velocity = Vector3.zero;

            agent.isStopped = true;
            anim.SetBool("Run", false);
            Attack();
        }
    }

    private void ApplySpotlightSettings()
    {
        soldierSettings.spotLight.range = soldierSettings.viewDistance;
        soldierSettings.originalSpotLightColor = soldierSettings.spotLight.color;
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];

        int lastWaypointIndex = 0;
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (true)
        {

            if (!CanSeePlayer())
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, patrolSettings.speed * Time.deltaTime);

                //if (transform.position == targetWaypoint)
                //if (Mathf.Abs(Vector3.Distance(transform.position,targetWaypoint)) <= 0.2f)
                Vector3 distance = transform.position - targetWaypoint;
                distance.y = 0;
                if (Mathf.Abs(distance.magnitude) <= 0.5f)
                {
                    //Stop walking
                    anim.SetBool("Walk", false);

                    if (patrolSettings.returnSameDirection)
                    {
                        int placeHolderIndex;
                        if ((waypoints.Length - 1) > targetWaypointIndex && targetWaypointIndex > lastWaypointIndex)
                            placeHolderIndex = targetWaypointIndex + 1;
                        else if (targetWaypointIndex == 0)
                            placeHolderIndex = targetWaypointIndex + 1;
                        else
                            placeHolderIndex = targetWaypointIndex - 1;

                        lastWaypointIndex = targetWaypointIndex;
                        targetWaypointIndex = placeHolderIndex;
                    }
                    else
                    {
                        targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                    }

                    targetWaypoint = waypoints[targetWaypointIndex];
                    yield return new WaitForSeconds(patrolSettings.waitTime);
                    yield return StartCoroutine(TurnToFace(targetWaypoint));

                }
                else
                {
                    //Resume walking
                    anim.SetBool("Walk", true);
                }
                yield return null;
            }
            else
            {
                //Player detected
                anim.SetBool("PlayerSpotted", true);
                yield return StartCoroutine(TurnToFace(player.transform.position));
                yield return new WaitForSeconds(3);
                yield return StartCoroutine(TurnToFace(targetWaypoint));

            }
        }
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        float turningAngle = Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle);

        anim.SetFloat("TurnAngle", turningAngle);
        anim.SetBool("Turn", true);

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, patrolSettings.turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }

    }

    bool CanSeePlayer()
    {
        float angle = 0;
        //Check if player in view distance
        if (Vector3.Distance(transform.position, playerPosition.position) < soldierSettings.viewDistance)
        {
            Vector3 dirToPlayer = (playerPosition.position - transform.position).normalized;
            float angleBetweenSoldierAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            angle = angleBetweenSoldierAndPlayer;
            //Check if player in view angle
            if (angleBetweenSoldierAndPlayer < soldierSettings.viewAngle / 2f)
            {
                //Check if player in LOS
                if (PlayerInLOS())
                    return true;

            }
        }
        //PLAYER NOT SPOTTED
        return false;
    }


    void Attack()
    {
        if (shootReady <= 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("AimIdle"))
        {
            anim.SetBool("Shoot", true);
            GetComponent<AudioSource>().PlayOneShot(combatSettings.attackSound);
            if (Random.Range(0f, 1f) <= combatSettings.accuracy)
            {
                Debug.Log("hit");
                player.GetComponent<Player>().PlayerHit(combatSettings.damage);
            }
            else
            {
                Debug.Log("miss");
            }
            shootReady = combatSettings.attackCooldown;
        }

    }


    private void OnDrawGizmos()
    {
        if (!patrolSettings.isStationary)
        {
            Vector3 startPosition = patrolSettings.soldierPath.GetChild(0).position;
            Vector3 previousPosition = startPosition;

            foreach (Transform waypoint in patrolSettings.soldierPath)
            {
                Gizmos.DrawSphere(waypoint.position, .3f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }

            if (!patrolSettings.returnSameDirection)
                Gizmos.DrawLine(previousPosition, startPosition);
        }
    }

    [System.Serializable]
    class PatrolSettings
    {
        public float speed = 0.0f;
        public float waitTime = 5.0f;
        public bool isStationary = false;
        public bool returnSameDirection = true;
        public float turnSpeed = 90f;
        public Transform soldierPath;
        public float timeToSpotPlayer = 1f;
    }

    [System.Serializable]
    class SoldierSettings
    {
        public Light spotLight;
        [System.NonSerialized]
        public Color originalSpotLightColor;
        public float viewDistance;
        public float viewAngle;
    }

}
