using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DefeatReason : MonoBehaviour
{

    GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.GetCurrentScene();

        string message;

        switch (gameManager.GetCurrentScene())
        {
            case (int) SceneIndexes.MAGMACORP:
                message = "Has sido detectado";
                break;
            case (int) SceneIndexes.TEAM_ALPHA:
                message = "Has muerto";
                break;
            case (int) SceneIndexes.PROTECTION:
                message = "Has muerto";
                break;
            default:
                message = "Has perdido";
                break;
        }

        GetComponent<Text>().text = message;
    }


}
