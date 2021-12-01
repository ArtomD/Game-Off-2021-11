using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    

    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPrefs.HasKey("LastLevel"));
        if (PlayerPrefs.HasKey("LastLevel") )
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel()
    {
        SceneLoader.LoadLevel(PlayerPrefs.GetInt("LastLevel"));
    }
}
