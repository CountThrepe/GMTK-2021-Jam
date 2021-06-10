using UnityEngine;
using UnityEngine.Audio;

public class PlayerPreferences : MonoBehaviour
{
    public AudioMixer mixer;
    private float volume = 1;
    private static PlayerPreferences self = null;

    void Start()
    {
        DontDestroyOnLoad(this);
        SetVolume(volume);
        self = GetComponent<PlayerPreferences>();
    }

    public static PlayerPreferences GetInstance()
    {
        return self;
    }
    
    public void SetVolume(float val)
    {
        volume = val;
        mixer.SetFloat("Volume", Mathf.Log10(val) * 20);
    }

    public float GetVolume()
    {
        return volume;
    }
}
