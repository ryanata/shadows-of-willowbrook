using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueState
{
    public string name;
    public bool baseDialogue;
    public bool clueAFound;
    public bool clueBFound;
    public bool clueCFound;
    public bool night;
}

[CreateAssetMenu(fileName = "SceneInfo", menuName = "Persistence")]
public class SceneInfo : ScriptableObject
{
    public Vector2 startPosition;
    // Set of clues found in this scene.
    public bool[] cluesFound = new bool[8];
    // State of dialogue with every villager.
    public List<DialogueState> dialogueRead = new List<DialogueState>
    {
        new DialogueState { name = "Police", baseDialogue = false, clueAFound = false, clueBFound = false, clueCFound = false, night = false },
        new DialogueState { name = "Mayor", baseDialogue = false, clueAFound = false, clueBFound = false, clueCFound = false, night = false },
        new DialogueState { name = "Samuel", baseDialogue = false, clueAFound = false, clueBFound = false, clueCFound = false, night = false },
        new DialogueState { name = "Baker", baseDialogue = false, clueAFound = false, clueBFound = false, clueCFound = false, night = false },
        new DialogueState { name = "Lillian", baseDialogue = false, clueAFound = false, clueBFound = false, clueCFound = false, night = false },
        new DialogueState { name = "Walter", baseDialogue = false, clueAFound = false, clueBFound = false, clueCFound = false, night = false },
    };
}
