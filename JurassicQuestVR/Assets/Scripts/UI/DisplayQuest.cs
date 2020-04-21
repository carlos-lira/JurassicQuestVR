using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayQuest : MonoBehaviour
{

    public int questId;
    public Text title;
    public Text history;
    public Text objectives;
    public Image image;

    public GameObject objectivesArea;
    public GameObject historyArea;


    public void SetQuest(Quest quest)
    {
        objectivesArea.SetActive(true);
        historyArea.SetActive(false);

        string tempObjectives = "";
        foreach (string objective in quest.objectives)
        {
            tempObjectives += "- " + objective + "\n";
        }

        questId = quest.questId;
        title.text = quest.title;
        history.text = quest.history;
        objectives.text = tempObjectives;
        image.sprite = quest.image;

    }
}
