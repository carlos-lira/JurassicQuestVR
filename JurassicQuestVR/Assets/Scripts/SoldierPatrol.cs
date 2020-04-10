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
    private bool walk;
    private bool inCombat;
    private bool playerSpotted;
    private bool shoot;
    private bool run;
    private bool hit;
    private float turnAngle;
    private bool turn;
    private bool dead;

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
            walk = true;
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

                //Player Spotted
                anim.SetBool("PlayerSpotted", true);
                spotLight.color = Color.yellow;
                timeSpotted += Time.deltaTime;
            }
            else
            {
                playerDetected = false;

                //Back to patrolling
                anim.SetBool("PlayerSpotted", false);
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

    }



    public void EnterCombat() 
    {
        spotLight.color = Color.red;
        //Change the animator to the CombatStateMachine
        anim.SetBool("InCombat", true);
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
                    //Stop walking
                    anim.SetBool("Walk", false);

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
                //isWalking = true;
                //isSpotted = false;
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
