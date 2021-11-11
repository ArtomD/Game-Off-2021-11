using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1;
    [Range(.1f, 3f)]
    public float pitch = 1;

    
    private AudioSource source;

    public void Set(AudioSource newSource)
    {
        source = newSource;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
    }

    public void Play()
    {        
        source.Play();
    }
}
