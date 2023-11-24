using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnTrigger : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 startPosition;
    public SceneInfo playerStorage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerStorage.startPosition = startPosition;
            playerStorage.villagerTalking = "";
            SceneManager.LoadSceneAsync(sceneToLoad);
        }
    }
}
