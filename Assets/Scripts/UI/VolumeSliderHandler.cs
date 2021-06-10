using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderHandler : MonoBehaviour
{
    private bool init = false;
    void Update()
    {
        if(!init)
        {
            Slider s = GetComponent<Slider>();
            GetComponent<Slider>().value = PlayerPreferences.GetInstance().GetVolume();
            init = true;
        }
    }

    public void SetPreferences(float val)
    {
        PlayerPreferences.GetInstance().SetVolume(val);
    }
}
