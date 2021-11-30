using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Muter : MonoBehaviour
{
    private AudioManager am;
    public void ToggleMute()
    {
        Debug.Log("MUTING");
        am.ToggleMute();
    }

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        
        
        Debug.Log("MG" + am.GetMuteStatus());

        if (am.GetMuteStatus())
        {
            
            gameObject.GetComponent<Toggle>().isOn = false;
            am.Mute();
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
