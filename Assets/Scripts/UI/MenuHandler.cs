using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    private bool mainMenu;
    public PauseManager pauseManager;
    public MainMenuHandler trans;

    void Start() {
        mainMenu = (trans != null);
    }

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
