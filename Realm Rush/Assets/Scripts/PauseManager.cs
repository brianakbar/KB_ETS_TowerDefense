using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseCanvas;
    bool isPaused;

    void Update() {
        if(Input.GetKeyDown(KeyCode.P)) {
            if(isPaused) {
                ResumeGame();
            }
            else {
                PauseGame();
            }
        }
    }

    public void ResumeGame() {
        isPaused = false;
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    void PauseGame() {
        isPaused = true;
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
    }
} 
