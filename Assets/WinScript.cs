using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{
       
    
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlaySound(Sound.Name.Win);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
