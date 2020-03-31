using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LauraTeamAlpha : MonoBehaviour
{
    private TeamAlphaLogic teamAlphaLogic;
    private Animator anim;
    private NavMeshAgent agent;

    private bool phase2;
    public Transform phase2Path;

    private bool phase4;
    public Transform phase4Path;

    public Transform lastPh2Point;
    public Transform lastPh4Point;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        teamAlphaLogic = FindObjectOfType<TeamAlphaLogic>();
        phase2 = false;
        phase4 = false;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (phase2)
            WalkWaypoints(2);
        if (phase4)
            WalkWaypoints(4);

    }

    public void Phase2() 
    {
        //STAND UP

        //PLAY DIALOGUE & WALK

        //RUN
        phase2 = true;
        //STOP
    }

    public void Phase3()
    {
        //START DIALOGUE
    }

    public void Phase4()
    {
        //RUN
        //RunWaypoints(phase4Path);
        phase4 = true;
        //STOP & START DIALOGUE

    }


    /*
    private void RunWaypoints(Transform pathHolder)
    {
        Vector3[] path = new Vector3[pathHolder.childCount];
        for (int i = 0; i < pathHolder.childCount; i++)
            path[i] = pathHolder.GetChild(i).position;

        lastPh2Point = path[path.Length-1];
        phase2 = true;
        //StartCoroutine(FollowPath(path));
    }
    */

    private void WalkWaypoints(int phase)
    {

        switch (phase)
        {
            case 2:
                if (GetDistance(transform.position, lastPh2Point.position) > 1.5f)
                    agent.SetDestination(lastPh2Point.position);
                else
                {
                    teamAlphaLogic.phase2Finished = true;
                    phase2 = false;
                }
                break;
            case 4:
                if (GetDistance(transform.position, lastPh4Point.position) > 1.5f)
                    agent.SetDestination(lastPh4Point.position);
                else
                {
                    teamAlphaLogic.phase4Finished = true;
                    phase4 = false;
                }
                break;
        }
    }

    private float GetDistance(Vector3 v1, Vector3 v2)
    {
        //Ignoring Y axis
        Vector3 distance = v1 - v2;
        distance.y = 0;

        return Mathf.Abs(distance.magnitude);
    }
        /*
        IEnumerator FollowPath(Vector3[] waypoints)
        {

            bool pathFinished = false;
            transform.position = waypoints[0];

            int lastWaypointIndex = 0;
            int targetWaypointIndex = 1;
            Vector3 targetWaypoint = waypoints[targetWaypointIndex];

            while (!pathFinished)
            {
                transform.LookAt(targetWaypoint);
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, runSpeed * Time.deltaTime);

                Vector3 distance = transform.position - targetWaypoint;
                distance.y = 0;
                if (Mathf.Abs(distance.magnitude) <= 0.5f)
                {
                    if (targetWaypointIndex + 1 != waypoints.Length)
                    {
                        targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                        targetWaypoint = waypoints[targetWaypointIndex];
                    }
                    else 
                    {
                        Debug.Log("PATH FINISHED");
                        pathFinished = true;
                    }
                }
                yield return null;
            }

            yield return null;
        }

        private void OnDrawGizmos()
        {
            Vector3 startPosition = phase2Path.GetChild(0).position;
            Vector3 previousPosition = startPosition;

            foreach (Transform waypoint in phase2Path)
            {
                Gizmos.DrawSphere(waypoint.position, .3f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }

        }
        */
}
