using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LauraTEST : MonoBehaviour
{
    Animator anim;

    private bool isRunning = false;
    private bool isTalking = false;
    private bool isStanding = false;
    private bool isTurning = false;

    public float speed = 0.5f;

    private Vector3 destination;
    AudioSource audioSource;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    /*
    private void Update()
    {
        
        if (!isStanding && IsIdle())
            isStanding = true;

        if (isRunning && GetDistance(transform.position, destination) <= 1.5f)
            StopRunning();

        if (isTalking && !GetComponent<AudioSource>().isPlaying)
            StopTalking();
        
    }
    */

    public void StandUp()
    {
        anim.SetBool("Stand", true);
        isStanding = true;
    }


    public void Talk(AudioClip audioClip)
    {
        StartCoroutine(PlaySound(audioClip));
    }

    public void StartTalking()
    {
        isTalking = true;
        anim.SetBool("Talk", true);
    }

    public IEnumerator StopTalking()
    {
        anim.SetBool("Talk", false);
        while (!IsIdle())
        {
            yield return null;
        }
        isTalking = false;
    }

    public void Run(Transform path)
    {
        Vector3[] waypoints = GetPathWaypoints(path);
        this.destination = waypoints[waypoints.Length - 1];
        StartCoroutine(RunPath(waypoints));
    }

    public IEnumerator PlaySound(AudioClip audioClip)
    {
        while (!IsIdle())
        {
            yield return null;
        }

        StartTalking();
        audioSource.PlayOneShot(audioClip);

        while (audioSource.isPlaying)
        {
            yield return null; 
        }

        StartCoroutine(StopTalking());
    }

    public IEnumerator RunPath(Vector3[] waypoints)
    {
        while (!IsIdle())
        {
            yield return null;
        }

        StartRunning();

        waypoints[0] = transform.position;


        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (targetWaypointIndex != 0)
        {
            Debug.Log("Speed: " + speed);
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
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

        StartCoroutine(StopRunning());
    }

    public void StartRunning()
    {
        isRunning = true;
        anim.SetBool("Run", true);
    }

    public IEnumerator StopRunning()
    {
        anim.SetBool("Run", false);
        while (!IsIdle())
        {
            yield return null;
        }
        isRunning = false;
    }
    public void TalkSound(AudioClip audioClip)
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
        return (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !isTurning);
    }

    public bool IsTurning()
    {
        return isTurning;
    }

    public void FaceTarget(Vector3 target)
    {
        transform.LookAt(target);
    }

    public IEnumerator TurnToFace(Vector3 lookTarget)
    {
        isTurning = true;
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        float turningAngle = Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle);

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, 360 * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
        isTurning = false;
    }

    public Vector3[] GetPathWaypoints(Transform path)
    {
        Vector3[] waypoints = new Vector3[path.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = path.GetChild(i).position;
        }

        return waypoints;
    }
}
