using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource caveAmbiance, windAmbiance, musicSource;
    public float maxAmbianceVolume, maxMusicVolume, windMultiplier = 1;
    public AudioClip[] musicTracks;

    private float ambianceVolume, deltaAmbiance;
    private float musicVolume, deltaMusic;
    private int musicIntensity;
    private const float t1 = 5, t2 = 15;

    private static MusicManager self;

    void Start()
    {
        self = GetComponent<MusicManager>();
        ambianceVolume = 0;
        deltaAmbiance = 0;
        musicVolume = 0;
        deltaMusic = 0;
        musicIntensity = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if(deltaAmbiance != 0) 
        {
            ambianceVolume = Mathf.Clamp(ambianceVolume + (deltaAmbiance * Time.deltaTime), 0, maxAmbianceVolume);
            if(ambianceVolume == 0 || ambianceVolume == maxAmbianceVolume) deltaAmbiance = 0;
        }

        if(deltaMusic != 0)
        {
            musicVolume = Mathf.Clamp(musicVolume + (deltaMusic * Time.deltaTime), 0, maxMusicVolume);
            if(musicVolume == 0 || musicVolume == maxMusicVolume) deltaMusic = 0;
            musicSource.volume = musicVolume;
        }

        if(transform.position.x < t1)
        {
            windAmbiance.volume = 0;
            caveAmbiance.volume = ambianceVolume;
        }
        else if(transform.position.x < t2)
        {
            float blend = ambianceVolume * (transform.position.x - t1) / (t2 - t1);
            caveAmbiance.volume = ambianceVolume - blend;
            windAmbiance.volume = blend * windMultiplier;
        }
        else
        {
            caveAmbiance.volume = 0;
            windAmbiance.volume = ambianceVolume * windMultiplier;
        }
    }

    public static MusicManager GetInstance()
    {
        return self;
    }

    public void HushBackground(float time)
    {
        deltaAmbiance = -ambianceVolume / time;
        deltaMusic = -musicVolume / time;
    }

    public void TurnUpBackground(float time)
    {
        deltaAmbiance = (maxAmbianceVolume - ambianceVolume) / time;
        deltaMusic = (maxMusicVolume - musicVolume) / time;

    }

    public void IncreaseMusicIntensity()
    {
        musicIntensity++;
        musicSource.clip = musicTracks[musicIntensity];
        musicSource.Play();
    }
}
