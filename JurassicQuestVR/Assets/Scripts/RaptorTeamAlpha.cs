using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorTeamAlpha : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Animator>().SetInteger("Idle", 7);
    }


}
