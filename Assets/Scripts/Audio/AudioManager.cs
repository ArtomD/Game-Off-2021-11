using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    
    public Sound[] sounds;

    [Range(0f, 1f)]
    public float masterVolume = 1;

    [SerializeField]
    public static AudioClip playerDeath;

    public static AudioManager Instance { get { return instance; } }

    public void Awake()
    {
        if (instance != null && instance != this)        
            Destroy(this.gameObject);        
        else        
            instance = this;      

        DontDestroyOnLoad(gameObject);        
    }

    private void Start()
    {
        foreach (Sound sound in sounds)        
            sound.Set(gameObject.AddComponent<AudioSource>());
        masterVolume = AudioListener.volume;
        PlaySound(Sound.Name.Soundtrack.ToString());
    }


    public void PlaySound(string id)
    {
        Array.Find(sounds, sound => sound.name.ToString() == id).Play();         
    }

    public void UpdateVolue(float volume)
    {
        masterVolume = volume;
        AudioListener.volume = masterVolume;        
    }

    public float GetVolume()
    {
        return masterVolume;
    }


}
