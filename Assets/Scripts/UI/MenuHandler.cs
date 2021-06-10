using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public bool mainMenu = true;
    public PauseManager pauseManager;
    public MainMenuHandler trans;


    public void Play()
    {
        if(mainMenu) trans.StartTransition();
        else pauseManager.TogglePause();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
