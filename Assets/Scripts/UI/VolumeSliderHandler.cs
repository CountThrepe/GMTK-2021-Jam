using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderHandler : MonoBehaviour
{
    private bool init = false;
    private bool exists = false;

    void Start() {
        exists = GameObject.FindWithTag("Preferences") != null;
    }

    void Update()
    {
        if(!exists) return;

        if(!init)
        {
            Slider s = GetComponent<Slider>();
            GetComponent<Slider>().value = PlayerPreferences.GetInstance().GetVolume();
            init = true;
        }
    }

    public void SetPreferences(float val)
    {
        if(!exists) return;
        
        PlayerPreferences.GetInstance().SetVolume(val);
    }
}
