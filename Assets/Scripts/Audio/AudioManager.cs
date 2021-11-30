using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public Sound[] sounds;

    [Range(0f, 1f)]
    public float masterVolume = 1;
    private float holdVolume = 1;
    private bool mute = false;
    private float muteVolumeHold;

    [SerializeField]
    public static AudioClip player;

    public static AudioManager Instance { get { return instance; } }

    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.Set(gameObject.AddComponent<AudioSource>());
        }

        //masterVolume = AudioListener.volume;        
    }

    private void Start()
    {

        PlaySound(Sound.Name.Soundtrack);
        PlaySound(Sound.Name.Ambience);
    }


    public void PlaySound(Sound.Name name)
    {
        //Debug.Log("Sound playing: " + name.ToString());
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if (sound != null)
        {
            sound.Play();

        }
        else
        {
            Debug.LogWarning("No sound setup for " + name.ToString());
        }





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

    public void Mute()
    {
        if(muteVolumeHold <= AudioListener.volume)
        {
            muteVolumeHold = AudioListener.volume;
        }        
        UpdateVolue(0);
        mute = true;        
    }

    public void Unmute()
    {
        UpdateVolue(holdVolume);
        mute = false;
    }

    public void ToggleMute()
    {
        if (mute)
            Unmute();
        else
            Mute();
    }

    internal void Pause(Sound.Name name)
    {

        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if (sound != null)
        {
            sound.Pause();

        }
        else
        {
            Debug.LogWarning("No sound setup for " + name.ToString());
        }

    }

    internal void UnPause(Sound.Name name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if (sound != null)
        {
            sound.UnPause();

        }
        else
        {
            Debug.LogWarning("No sound setup for " + name.ToString());
        }
    }

    public bool GetMuteStatus()
    {
        return mute;
    }
}
