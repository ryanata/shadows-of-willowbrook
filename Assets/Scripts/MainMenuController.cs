using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public SceneInfo playerStorage;
    public void PlayGame()
    {
        // Reset the scriptable object
        playerStorage.ResetVariables();
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
