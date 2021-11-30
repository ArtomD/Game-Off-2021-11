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

        LoadAudioSettings();
    }

    private void Start()
    {
        PlaySound(Sound.Name.Soundtrack);
        PlaySound(Sound.Name.Ambience);

    }

    public void LoadAudioSettings()
    {        
        if (PlayerPrefs.HasKey("Volume"))            
            UpdateVolue(PlayerPrefs.GetFloat("Volume"));
        else
        {
            UpdateVolue(AudioListener.volume);
        }

        if (PlayerPrefs.HasKey("Mute"))
        {
            if(PlayerPrefs.GetInt("Mute") == 1 && !mute)
            {
                Mute();
            }
            else if(PlayerPrefs.GetInt("Mute") != 1 && mute)
            {
                Unmute();
            }
        }


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
        if (volume > 0 && mute)
        {
            mute = false;
            PlayerPrefs.SetInt("Mute", 0);
        }            
        masterVolume = volume;
        AudioListener.volume = masterVolume;
        PlayerPrefs.SetFloat("Volume", masterVolume);
    }

    public float GetVolume()
    {
        return masterVolume;
    }

    public void Mute()
    {
        PlayerPrefs.SetInt("Mute", 1);
        mute = true;
        if (muteVolumeHold <= AudioListener.volume)
        {
            muteVolumeHold = AudioListener.volume;
        }        
        UpdateVolue(0);
            
    }

    public void Unmute()
    {
        UpdateVolue(holdVolume);
        mute = false;
        PlayerPrefs.SetInt("Mute", 0);
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
