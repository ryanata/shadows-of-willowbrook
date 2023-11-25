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

[System.Serializable]
public class Destination
{
    public Vector3 position;
    public string sceneName;
}

[System.Serializable]
public class Schedule
{
    public string name;
    public List<Destination> destinations;
    public Vector3 homeEntrance;
    public Vector3 homeExit;
    public int curIndex;
}

[CreateAssetMenu(fileName = "SceneInfo", menuName = "Persistence")]
public class SceneInfo : ScriptableObject
{
    public Vector2 startPosition;
    // Set of clues found in this scene.
    // 0 = Journal, 1 = Letter, 2 = Knife, 3 = Poisonous Flower, 4 = Note from diary, 5 = Potion, 6 = Blueprint, 7 = Key
    public bool[] cluesFound = new bool[8];
    // State of dialogue with every villager.
    public List<DialogueState> dialogueRead = new List<DialogueState>
    {
        new DialogueState { name = "Police", baseDialogue = false, clueAFound = false, clueBFound = false, clueCFound = false, night = false },
        new DialogueState { name = "Mayor", baseDialogue = false, clueAFound = false, clueBFound = false, clueCFound = false, night = false },
        new DialogueState { name = "Samuel", baseDialogue = false, clueAFound = false, clueBFound = false, clueCFound = false, night = false },
        new DialogueState { name = "Isabel", baseDialogue = false, clueAFound = false, clueBFound = false, clueCFound = false, night = false },
        new DialogueState { name = "Lillian", baseDialogue = false, clueAFound = false, clueBFound = false, clueCFound = false, night = false },
        new DialogueState { name = "Walter", baseDialogue = false, clueAFound = false, clueBFound = false, clueCFound = false, night = false },
    };
    // Create list of Shedules for each villager.
    public List<Schedule> schedules = new List<Schedule>
    {
        new Schedule { 
            name = "Police", 
            destinations = new List<Destination> {
                // Lake
                new Destination { 
                    position = new Vector3(15, 3, 0), 
                    sceneName = "MainScene" 
                },
                // Forest
                new Destination { 
                    position = new Vector3(-31, 6, 0), 
                    sceneName = "MainScene" 
                },
                // Furniture
                new Destination { 
                    position = new Vector3(3.5f, 0, 0), 
                    sceneName = "PoliceScene2"
                },
                // Desk
                new Destination { 
                    position = new Vector3(-3.5f, 0.6f, 0), 
                    sceneName = "PoliceScene2"
                },
            },
            homeEntrance = new Vector3(12.7f, 11.5f, 0),
            homeExit = new Vector3(0, -4.5f, 0),
            curIndex = 0 
        },
        new Schedule { 
            name = "Mayor", 
            destinations = new List<Destination> {
                // City Center
                new Destination { 
                    position = new Vector3(0, 8, 0), 
                    sceneName = "MainScene" 
                },
                // Police station
                new Destination { 
                    position = new Vector3(21, 10, 0), 
                    sceneName = "MainScene" 
                },
                // Bed
                new Destination { 
                    position = new Vector3(-31, 7, 0), 
                    sceneName = "MayorScene"
                },
            },
            homeEntrance = new Vector3(-14.5f, 27.25f, 0),
            homeExit = new Vector3(-33, 3.5f, 0),
            curIndex = 0 
        },
        new Schedule { 
            name = "Samuel", 
            destinations = new List<Destination> {
                // Forest, near florist
                new Destination { 
                    position = new Vector3(-13.7f, 2, 0), 
                    sceneName = "MainScene" 
                },
                // Near detective house
                new Destination { 
                    position = new Vector3(4.5f, 16, 0), 
                    sceneName = "MainScene" 
                },
                // Couch
                new Destination { 
                    position = new Vector3(-34.3f, 3.75f, 0), 
                    sceneName = "WriterScene"
                },
                // Bed/Desk
                new Destination { 
                    position = new Vector3(-26.5f, 3.75f, 0), 
                    sceneName = "WriterScene"
                },
            },
            homeEntrance = new Vector3(21.25f, 22.25f, 0),
            homeExit = new Vector3(-32, 1.7f, 0),
            curIndex = 0
        },
        new Schedule { 
            name = "Isabel", 
            destinations = new List<Destination> {
                // Mayor's House
                new Destination { 
                    position = new Vector3(-11, 24, 0), 
                    sceneName = "MainScene" 
                },
                // Forest
                new Destination { 
                    position = new Vector3(-5, 24, 0), 
                    sceneName = "MainScene" 
                },
                // Counter
                new Destination { 
                    position = new Vector3(-5.5f, -0.45f, 0), 
                    sceneName = "BakeryScene"
                },
                // Seating area
                new Destination { 
                    position = new Vector3(3.5f, 1, 0), 
                    sceneName = "BakeryScene"
                },
            },
            homeEntrance = new Vector3(4.5f, 31.5f, 0),
            homeExit = new Vector3(0, -4, 0),
            curIndex = 0 
        },
        new Schedule { 
            name = "Lillian",
            destinations = new List<Destination> {
                // Flower bed
                new Destination { 
                    position = new Vector3(-32, -4, 0), 
                    sceneName = "MainScene" 
                },
                // Walter's house
                new Destination { 
                    position = new Vector3(-41, 13, 0), 
                    sceneName = "MainScene" 
                },
                // Flower row
                new Destination { 
                    position = new Vector3(1.28f, -0.64f, 0), 
                    sceneName = "FlowerShopScene"
                },
                // Counter
                new Destination { 
                    position = new Vector3(-5, -0.25f, 0), 
                    sceneName = "FlowerShopScene"
                },
            },
            homeEntrance = new Vector3(-10.5f, 11, 0),
            homeExit = new Vector3(0, -4.5f, 0),
            curIndex = 0 
        },
        new Schedule { 
            name = "Walter", 
            destinations = new List<Destination> {
                // Lake
                new Destination { 
                    position = new Vector3(8, 1, 0), 
                    sceneName = "MainScene" 
                },
                // Backyard
                new Destination { 
                    position = new Vector3(-40, 28, 0), 
                    sceneName = "MainScene" 
                },
                // Bed
                new Destination { 
                    position = new Vector3(-29, 7, 0), 
                    sceneName = "WalterScene"
                },
                // Entrance
                new Destination { 
                    position = new Vector3(-35f, 7.5f, 0), 
                    sceneName = "WalterScene"
                },
            },
            homeEntrance = new Vector3(-34, 20.5f, 0),
            homeExit = new Vector3(-34, 4.8f, 0),
            curIndex = 0 
        }
    };
    public float dayDuration = 90f;
    public float nightDuration = 90f;
    public float transitionDuration = 30f;
    public string villagerTalking = "";
    public bool guessedRight = false;

    // Reset all non-static variables to their default values.
    public void ResetVariables()
    {
        startPosition = new Vector2(0, 0);
        // Reset cluesFound to be all false.
        for (int i = 0; i < cluesFound.Length; i++)
        {
            cluesFound[i] = false;
        }
        // Reset dialogueRead to be all false.
        foreach (DialogueState state in dialogueRead)
        {
            state.baseDialogue = false;
            state.clueAFound = false;
            state.clueBFound = false;
            state.clueCFound = false;
            state.night = false;
        }
        // Reset schedules to be all at the beginning.
        foreach (Schedule schedule in schedules)
        {
            schedule.curIndex = 0;
        }
        villagerTalking = "";
        guessedRight = false;
    }

}
