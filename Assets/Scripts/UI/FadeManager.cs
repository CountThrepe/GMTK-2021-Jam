using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public GameObject black; 
    private Image blackImage;
    private float opacity;
    private float fadeAmount;
    private bool finalFade;
    void Start()
    {
        blackImage = black.GetComponent<Image>();
        
        opacity = 1;
        finalFade = false;
    }
    void Update()
    {
        if(fadeAmount != 0)
        {
            opacity = Mathf.Clamp(opacity + (fadeAmount * Time.deltaTime), 0, 1);
            blackImage.color = new Vector4(0, 0, 0, opacity);
            if(opacity == 1) {
                fadeAmount = 0;
                if(finalFade) NextScene();
            }
            else if(opacity == 0)
            {
                fadeAmount = 0;
                black.SetActive(false);
            }
        }
    }

    public void FadeIn(float duration)
    {
        if(opacity == 0) return;

        if(duration == 0)
        {
            opacity = 0;
            blackImage.color = new Vector4(0, 0, 0, opacity);
            fadeAmount = 0;
            black.SetActive(false);
        }
        else fadeAmount = -opacity / duration;
    }

    public void FadeOut(float duration, bool final = false)
    {
        if(opacity == 1) 
        {
            if(final) NextScene();
            return;
        }

        finalFade = final;
        black.SetActive(true);
        if(duration == 0)
        {
            opacity = 1;
            blackImage.color = new Vector4(0, 0, 0, opacity);
            fadeAmount = 0;
        }
        else fadeAmount = (1 - opacity) / duration;
    }

    public void NextScene()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }
}
