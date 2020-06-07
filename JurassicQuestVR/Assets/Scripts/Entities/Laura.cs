using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Laura : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;
    PauseMenu pauseMenu;

    private bool isRunning;
    private bool isTalking = false;
    private bool isStanding;

    private Vector3 destination;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponentInChildren<NavMeshAgent>();
        pauseMenu = GameObject.Find("LevelManager").GetComponent<PauseMenu>();
    }

    private void Update()
    {
        if (!pauseMenu.GameIsPaused())
        {
            if (!isStanding && IsIdle())
                isStanding = true;

            if (isRunning && GetDistance(transform.position, destination) <= 1.5f)
                StopRunning();

            if (isTalking && !GetComponent<AudioSource>().isPlaying)
                StopTalking();
        }      
    }

    public void StandUp()
    {
        anim.SetBool("Stand", true);
    }
    public void Talk(AudioClip audioClip)
    {
        if (agent != null)
            agent.isStopped = true;
        isTalking = true;
        anim.SetBool("Talk", true);
        PlaySound(audioClip);
    }
    public void StopTalking()
    {
        GetComponent<AudioSource>().Stop();
        isTalking = false;
        anim.SetBool("Talk", false);
    }
    public void Run(Vector3 destination)
    {
        if (IsIdle())
        {
            isRunning = true;
            this.destination = destination;
            agent.SetDestination(destination);
            agent.isStopped = false;
            anim.SetBool("Run", true);
        }
    }

    public IEnumerator RunPath(Vector3[] waypoints)
    {
        isRunning = true;

        transform.position = waypoints[0];

        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);
        anim.SetBool("Run", true);

        while (targetWaypointIndex != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, 8 * Time.deltaTime);
            Vector3 distance = transform.position - targetWaypoint;
            distance.y = 0;
            if (Mathf.Abs(distance.magnitude) <= 0.5f)
            {
                //Stop walking
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;

                targetWaypoint = waypoints[targetWaypointIndex];
                transform.LookAt(targetWaypoint);

            }
            yield return null;
        }

        anim.SetBool("Run", false);
    }

    public void StopRunning()
    {
        isRunning = false;
        agent.isStopped = false;
        anim.SetBool("Run", false);
    }
    public void PlaySound(AudioClip audioClip)
    {
        GetComponent<AudioSource>().PlayOneShot(audioClip);
    }

    float GetDistance(Vector3 v1, Vector3 v2)
    {
        //Ignoring Y axis
        Vector3 distance = v1 - v2;
        distance.y = 0;

        return Mathf.Abs(distance.magnitude);
    }

    public void Teleport(Vector3 position)
    {
        agent.Warp(position);
    }

    public bool IsTalking()
    {
        return isTalking;
    }
    public bool IsRunning()
    {
        return isRunning;
    }

    public bool IsStanding()
    {
        return isStanding;
    }

    public bool IsIdle()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }

    public IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        float turningAngle = Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle);

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, 360 * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }

    }
}
