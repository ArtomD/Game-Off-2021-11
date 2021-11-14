using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    private AudioManager audioManager;
    private Slider slider;
    private float volume;

    // Start is called before the first frame update
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        audioManager = FindObjectOfType<AudioManager>();
        slider.value = audioManager.GetVolume();
    }

    // Update is called once per frame
    void Update()
    {
        if(slider.value != volume)
        {
            volume = slider.value;
            audioManager.UpdateVolue(volume);
        }
            
    }
}
