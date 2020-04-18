using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DefeatReason : MonoBehaviour
{

    void Start()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                GetComponent<Text>().text = "Has sido detectado";
                break;
            case 2:
                GetComponent<Text>().text = "Has muerto";
                break;
            case 3:
                GetComponent<Text>().text = "Has muerto";
                break;
            default:
                GetComponent<Text>().text = "Has perdido";
                break;
        }
    }


}
