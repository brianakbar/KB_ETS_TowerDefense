using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }

    public void LoadEasyLevel() {
        SceneManager.LoadScene(1);
    }

    public void LoadMediumLevel() {
        SceneManager.LoadScene(2);
    }

    public void LoadHardLevel() {
        SceneManager.LoadScene(3);
    }

    public void ReloadLevel() {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentBuildIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
