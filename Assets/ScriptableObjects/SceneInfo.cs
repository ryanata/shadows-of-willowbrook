using UnityEngine;

[CreateAssetMenu(fileName = "SceneInfo", menuName = "Persistence")]
public class SceneInfo : ScriptableObject
{
    public Vector2 startPosition;
    // Set of clues found in this scene.
    public bool[] cluesFound = new bool[8];
}
