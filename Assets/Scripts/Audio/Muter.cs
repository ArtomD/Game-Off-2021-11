using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muter : MonoBehaviour
{
    private AudioManager am;
    public void ToggleMute()
    {
        am.ToggleMute();
    }

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
