using UnityEngine;

[System.Serializable]
public class Sound
{
    public Name name;

    public bool loop;
    public enum Name
    {
        PlayerJump, // done
        PlayerSpawned,// done
        PlayerDied,// done
        PlayerDamaged,// done
        PlayerWalk,// done
        PlayerDash,// done
        PlayerDashAvailable,// done
        PanelCollide, // done
        PanelTurn,  // NOT SURE, how is it different than collide?
        PanelSlide, //NOT SURE, how is it different than collide?
        PanelSet,  // Done
        PanelUnset,// Done
        LevelComplete, // Done
        Soundtrack// done
    }

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
        source.loop = loop;
    }

    public void Play()
    {
        source.Play();

    }

    public void Pause()
    {
        if (source.isPlaying)
        {

            source.Pause();
        }

    }

    public void UnPause()
    {
        if (!source.isPlaying)
        {
            Play();
        } else
        {
            source.UnPause();
        }
        

    }
}
