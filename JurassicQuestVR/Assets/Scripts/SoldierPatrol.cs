using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierPatrol : MonoBehaviour
{

    //public GameManager gameManager;
    public LevelManager levelManager;

    public float speed = 0.0f;
    public float waitTime = 5.0f;
    public bool isStationary = false;
    public bool returnSameDirection = true;
    public float turnSpeed = 90f;
    public Transform pathHolder;
    public float timeToLose = 1f;

    public bool combatEnabled;

    public Light spotLight;
    private Color originalSpotLightColor;
    public float viewDistance;
    public float viewAngle;
    public LayerMask viewMask;

    private GameObject player;
    private Transform playerPosition;

    private float timeSpotted = 0f;

    private Animator anim;
    private bool isWalking = false;
    private bool isTurningAroundLeft = false;
    private bool isTurningAroundRight = false;
    private bool isTurningRight = false;
    private bool isTurningLeft = false;
    private bool isDead = false;
    private bool isDamaged = false;
    private bool isShooting = false;
    private bool isSpotted = false;

    private bool playerDetected = false;
    private bool playerEngageCombat = false;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player.transform;

        anim = gameObject.GetComponentInChildren<Animator>();

        spotLight.range = viewDistance;
        originalSpotLightColor = spotLight.color;

        if (pathHolder == null)
            isStationary = true;

        if (!isStationary)
        {
            isWalking = true;
            Vector3[] waypoints = new Vector3[pathHolder.childCount];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = pathHolder.GetChild(i).position;
            }

            StartCoroutine(FollowPath(waypoints));
        }
    }

    private void Update()
    {

        if (timeSpotted < timeToLose)
        {
            if (CanSeePlayer())
            {
                playerDetected = true;
                spotLight.color = Color.yellow;
                timeSpotted += Time.deltaTime;
            }
            else
            {
                playerDetected = false;
                timeSpotted -= Time.deltaTime;
            }
        }
        else 
        {
            spotLight.color = Color.red;
            if (combatEnabled)
            {
                EnterCombat();
            }
            else
            {
                levelManager.EndGame(false);
            }
        }

        timeSpotted = Mathf.Clamp(timeSpotted, 0, timeToLose);

        if (timeSpotted == 0)
            spotLight.color = originalSpotLightColor;

        Animate();

    }

    private void SetAllAnimationsFalse()
    {
        isWalking = false;
        isTurningLeft = false;
        isTurningAroundLeft = false;
        isTurningRight = false;
        isTurningAroundRight = false;
        isSpotted = false;
        isShooting = false;
        isDamaged = false;
        isDead = false;
    }

    private void Animate() 
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isTurningLeft", isTurningLeft);
        anim.SetBool("isTurningRight", isTurningRight);
        anim.SetBool("isTurningAroundLeft", isTurningAroundLeft);
        anim.SetBool("isTurningAroundRight", isTurningAroundRight);
        anim.SetBool("isSpotted", isSpotted);
        anim.SetBool("isShooting", isShooting);
        anim.SetBool("isDamaged", isDamaged);
        anim.SetBool("isDead", isDead);
    }

    public void EnterCombat() 
    {
        spotLight.color = Color.red;
        SetAllAnimationsFalse();
        Animate();
        GetComponent<EnemyAttack>().enabled = true;
        StopAllCoroutines();
        GetComponent<SoldierPatrol>().enabled = false;
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];

        int lastWaypointIndex = 0;
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (true) {

            if (!CanSeePlayer())
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);

                //if (transform.position == targetWaypoint)
                //if (Mathf.Abs(Vector3.Distance(transform.position,targetWaypoint)) <= 0.2f)
                Vector3 distance = transform.position - targetWaypoint;
                distance.y = 0;
                if (Mathf.Abs(distance.magnitude) <= 0.5f)
                {
                    isWalking = false;

                    if (returnSameDirection)
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
                    yield return new WaitForSeconds(waitTime);
                    yield return StartCoroutine(TurnToFace(targetWaypoint));

                }
                else
                {
                    isWalking = true;
                    isSpotted = false;
                    isTurningRight = false;
                    isTurningLeft = false;
                    isTurningAroundLeft = false;
                    isTurningAroundRight = false;
                }
                yield return null;
            }
            else
            {
                isWalking = false;
                isSpotted = true;
                isTurningRight = false;
                isTurningLeft = false;
                isTurningAroundLeft = false;
                isTurningAroundRight = false;
                yield return StartCoroutine(TurnToFace(player.transform.position));
                yield return new WaitForSeconds(3);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
                isWalking = true;
                isSpotted = false;
            }
        }
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;
        
        float turningAngle = Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle);
        
        if (turningAngle < -120 || turningAngle > 120)
        {
            if (turningAngle < 0)
                isTurningAroundLeft = true;
            else
                isTurningAroundRight = true;
        }
        else if (turningAngle < 0)
        {
            isTurningLeft = true;
        }
        else
        {
            isTurningRight = true;
        }
        
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        } 
        
    }



    private void OnDrawGizmos()
    {
        if (!isStationary)
        {
            Vector3 startPosition = pathHolder.GetChild(0).position;
            Vector3 previousPosition = startPosition;

            foreach (Transform waypoint in pathHolder)
            {
                Gizmos.DrawSphere(waypoint.position, .3f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }

            if (!returnSameDirection)
                Gizmos.DrawLine(previousPosition, startPosition);
        }
    }

    bool CanSeePlayer() 
    {
        float angle = 0;
        //Check if player in view distance
        if (Vector3.Distance(transform.position, playerPosition.position) < viewDistance)
        {
            Vector3 dirToPlayer = (playerPosition.position - transform.position).normalized;
            float angleBetweenSoldierAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            angle = angleBetweenSoldierAndPlayer;
            //Check if player in view angle
            if (angleBetweenSoldierAndPlayer < viewAngle / 2f)
            {
                //Check if player in LOS
                if (!Physics.Linecast(transform.position, playerPosition.position, viewMask, QueryTriggerInteraction.Ignore))
                {
                    //PLAYER SPOTTED
                    return true;
                }

            }
        }

        //Debug.Log("Distance: " + Vector3.Distance(transform.position, playerPosition.position));
        //Debug.Log("Angle: " + angle);
        //PLAYER NOT SPOTTED
        return false;
    }

}
