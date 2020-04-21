using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateMainCanvas : MonoBehaviour
{
    public GameObject[] quests;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateActiveQuests();
    }

    public void UpdateActiveQuests()
    {
        int playerProgress = GameObject.Find("GameManager").GetComponent<GameManager>().playerProgress;

        DeactivateAllQuests();

        for (int i = 0; i < quests.Length; i++)
        {
            if (i <= playerProgress)
                quests[i].GetComponent<Button>().interactable = true;
            else
                quests[i].GetComponent<Button>().interactable = false;
        }
    }

    void DeactivateAllQuests()
    {
        for (int i = 0; i < quests.Length; i++)
        {
            quests[i].GetComponent<Button>().interactable = false;
        }
    }
}
