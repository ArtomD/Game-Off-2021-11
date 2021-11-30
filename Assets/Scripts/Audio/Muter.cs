using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Muter : MonoBehaviour
{
    private AudioManager am;
    public void ToggleMute()
    {        
        if(am != null)
            am.ToggleMute();
    }

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        
        if (am.GetMuteStatus())
        {            
            gameObject.GetComponent<Toggle>().SetIsOnWithoutNotify(false);            
        }
        am.LoadAudioSettings();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
