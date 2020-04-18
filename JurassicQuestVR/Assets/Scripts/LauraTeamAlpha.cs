using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LauraTeamAlpha : MonoBehaviour
{
    private TeamAlphaLogicOLD teamAlphaLogic;
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
        teamAlphaLogic = FindObjectOfType<TeamAlphaLogicOLD>();
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


    private void WalkWaypoints(int phase)
    {

        switch (phase)
        {
            case 2:
                if (GetDistance(transform.position, lastPh2Point.position) > 1.5f)
                {
                    agent.SetDestination(lastPh2Point.position);
                }
                else
                {
                    teamAlphaLogic.phase2Finished = true;
                    phase2 = false;
                }
                break;
            case 4:
                if (GetDistance(transform.position, lastPh4Point.position) > 1.5f)
                {
                    agent.SetDestination(lastPh4Point.position);
                    anim.SetBool("Run", true);
                }
                else
                {
                    anim.SetBool("Run", false);
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

}
