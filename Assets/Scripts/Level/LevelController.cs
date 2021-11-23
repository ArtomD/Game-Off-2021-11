using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    

    public GameObject levelText;
    public GameObject levelTime;
    public GameObject levelTimeCompleted;
    public int restartHoldDuration = 2;
    public Slider slider;
    private DateTime restartTimer;
    private bool keyDown = false;    
    public GameObject winScreen;

    public string levelName;

    private DateTime time;
    private float elapsedTime;
    private bool won = false;


    // Start is called before the first frame update
    void Start()
    {      
        winScreen.SetActive(false);
        time = DateTime.UtcNow;
        levelText.GetComponent<TextMeshProUGUI>().text = levelName;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!won)
            elapsedTime = (float)(DateTime.UtcNow - time).TotalSeconds;

        levelTime.GetComponent<TextMeshProUGUI>().text = "Elapsed Time - " + Mathf.Round(elapsedTime * 100.0f ) / 100.0f;
        levelTimeCompleted.GetComponent<TextMeshProUGUI>().text = "total time " + Mathf.Round(elapsedTime * 100.0f) / 100.0f + " seconds";

        if (Input.GetKeyDown(KeyCode.R))
            keyDown = true;

        if (Input.GetKeyUp(KeyCode.R))
            keyDown = false;

        if (!keyDown)
            restartTimer = DateTime.UtcNow;

        

              

        slider.value = (float)(DateTime.UtcNow - restartTimer).TotalSeconds / restartHoldDuration;

        if ((DateTime.UtcNow - restartTimer).TotalSeconds > restartHoldDuration && keyDown)
        {
            SceneLoader.RestartLevel();
        }
    }

    public void Win()
    {
        won = true;
        StartCoroutine(WinIEnum());        
    }

    private IEnumerator WinIEnum()
    {
        FindObjectOfType<Player>().GetComponent<Disolver>().Out();
        yield return new WaitForSeconds(1);
        FindObjectOfType<Player>().gameObject.SetActive(false);          
        yield return new WaitForSeconds(0.1f);        
        winScreen.SetActive(true);
        //SceneLoader.LoadNextLevel();
    }

    public void StopTimer()
    {

    }


}
