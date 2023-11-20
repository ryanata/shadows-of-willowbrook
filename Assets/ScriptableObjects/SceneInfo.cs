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
                    position = new Vector3(3.5f, -1, 0), 
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
                // Police
                new Destination { 
                    position = new Vector3(21, 10, 0), 
                    sceneName = "MainScene" 
                },
                // Furniture
                new Destination { 
                    position = new Vector3(3.5f, -1, 0), 
                    sceneName = "PoliceScene2"
                },
                // Desk
                new Destination { 
                    position = new Vector3(-3.5f, 0.6f, 0), 
                    sceneName = "PoliceScene2"
                },
            },
            homeEntrance = new Vector3(-14.5f, 27.25f, 0),
            homeExit = new Vector3(0, -4.5f, 0),
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
                // Furniture
                new Destination { 
                    position = new Vector3(3.5f, -1, 0), 
                    sceneName = "PoliceScene2"
                },
                // Desk
                new Destination { 
                    position = new Vector3(-3.5f, 0.6f, 0), 
                    sceneName = "PoliceScene2"
                },
            },
            homeEntrance = new Vector3(21.25f, 22.25f, 0),
            homeExit = new Vector3(0, -4.5f, 0),
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
                // Furniture
                new Destination { 
                    position = new Vector3(3.5f, -1, 0), 
                    sceneName = "PoliceScene2"
                },
                // Desk
                new Destination { 
                    position = new Vector3(-3.5f, 0.6f, 0), 
                    sceneName = "PoliceScene2"
                },
            },
            homeEntrance = new Vector3(4.5f, 31.5f, 0),
            homeExit = new Vector3(0, -4.5f, 0),
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
                // Furniture
                new Destination { 
                    position = new Vector3(3.5f, -1, 0), 
                    sceneName = "PoliceScene2"
                },
                // Desk
                new Destination { 
                    position = new Vector3(-3.5f, 0.6f, 0), 
                    sceneName = "PoliceScene2"
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
                // Furniture
                new Destination { 
                    position = new Vector3(3.5f, -1, 0), 
                    sceneName = "PoliceScene2"
                },
                // Desk
                new Destination { 
                    position = new Vector3(-3.5f, 0.6f, 0), 
                    sceneName = "PoliceScene2"
                },
            },
            homeEntrance = new Vector3(-34, 20.5f, 0),
            homeExit = new Vector3(0, -4.5f, 0),
            curIndex = 0 
        }
    };
    public float dayDuration = 90f;
    public float nightDuration = 90f;
    public float transitionDuration = 30f;
}
