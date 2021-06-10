using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CutsceneScript : MonoBehaviour
{

    void Update()
    {
        if(Keyboard.current.escapeKey.isPressed) NextScene();
    }
    
    public void NextScene()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }
}
