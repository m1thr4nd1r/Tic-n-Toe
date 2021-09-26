using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{
    public static void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public static void BackToTitleScreen()
    {
        SceneManager.LoadScene(0);
    }

    public static void ExitGame()
    {
        Application.Quit();
    }
}
