using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Quest : ScriptableObject
{
    public int questId;
    public string title;
    public string history;
    public List<string> objectives;
    public Sprite image;
}
