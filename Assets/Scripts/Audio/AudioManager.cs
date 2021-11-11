using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    
    public Sound[] sounds;

    [Range(0f, 1f)]
    public float masterVolume = 1;
    
    [Range(.1f, 3f)]
    public float defaultPitch = 1;

    [SerializeField]
    public static AudioClip playerDeath;

    public static AudioManager Instance { get { return instance; } }

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
        
    }

    private void Start()
    {
        foreach (Sound sound in sounds)
        {
            sound.Set(gameObject.AddComponent<AudioSource>());
        }        
    }


    public void PlaySound(string id)
    {
        Array.Find(sounds, sound => sound.name == id).Play();         
    }

    


}
