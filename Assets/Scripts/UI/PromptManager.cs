using UnityEngine;
using UnityEngine.UI;

public class PromptManager : MonoBehaviour
{
    public Text text;
    private float promptEnd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(text.enabled == true)
        {
            if(Time.time > promptEnd)
            {
                text.enabled = false;
            }
        }
    }

    public void ShowPrompt(string textToShow, float time)
    {
        text.text = textToShow;
        text.enabled = true;
        promptEnd = Time.time + time;
    }
}
