using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool paused = false;
    
    public void TogglePause()
    {
        if(paused) Unpause();
        else Pause();
        paused = !paused;
    }

    public bool IsPaused()
    {
        return paused;
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Unpause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
