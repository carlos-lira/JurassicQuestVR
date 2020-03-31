using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamAlphaLogic : LevelManager
{
    public GameObject player;
    public GameObject laura;
    public Transform lauraPhase2EndPosition;
    public GameObject[] boundaries;

    private int phase;

    public bool phase2Finished;
    public bool phase4Finished;

    private void Start()
    {
        phase = 1;
        phase2Finished = false;
        phase4Finished = false;
        laura = GameObject.Find("Laura");
        player = GameObject.Find("OVRPlayerController");
    }


    private void Update()
    {

        Debug.LogWarning("Phase: " + phase);
        if (phase == 1 && deadEnemies >= 3 && Vector3.Distance(laura.transform.position, player.transform.position) < 10f)
            EnterPhase2();

        if (phase == 2 && phase2Finished)
            EnterPhase3();

        if (phase == 3 && deadEnemies >= 5)
            EnterPhase4();

        if (phase == 4 && phase4Finished)
            EndGame(true);

    }

    private void EnterPhase2()
    {
        phase = 2;
        boundaries[0].SetActive(false);
        laura.GetComponent<LauraTeamAlpha>().Phase2();
    }

    private void EnterPhase3()
    {
        phase = 3;
        boundaries[1].SetActive(false);
        laura.GetComponent<LauraTeamAlpha>().Phase3();
    }

    private void EnterPhase4()
    {
        phase = 4;
        laura.GetComponent<LauraTeamAlpha>().Phase4();
    }


    public void StartPhase3()
    {
        EnterPhase3();
    }
}
