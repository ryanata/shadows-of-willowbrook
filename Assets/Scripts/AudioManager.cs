using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource themeMusic;
    public AudioSource bossMusic;

    private bool bossMusicPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this; // this is the current instance of the script
            DontDestroyOnLoad(gameObject); // don't destroy this game object when loading a new scene
        }
        else
        {
            Destroy(gameObject); // destroy the game object if there is already an instance of this script
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "SecretLayerScene" && !bossMusicPlaying)
        {
            Debug.Log("Boss music playing, theme music stopped");
            themeMusic.Stop();
            bossMusic.Play();
            bossMusicPlaying = true;
        }
        else if (SceneManager.GetActiveScene().name != "SecretLayerScene" && bossMusicPlaying)
        {
            Debug.Log("Theme music playing, boss music stopped");
            bossMusic.Stop();
            themeMusic.Play();
            bossMusicPlaying = false;
        }
    }
}
