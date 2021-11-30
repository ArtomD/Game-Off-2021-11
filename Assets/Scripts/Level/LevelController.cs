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
    public float restartHoldDuration = 2;
    public Slider slider;
    private DateTime restartPressedAt;
    public GameObject winScreen;

    public string levelName;

    private DateTime time;
    private float elapsedTime;
    private Player _player;

    private enum LevelState  {
        Won,
        Lost,
        InProgress,
    }

    private LevelState levelState = LevelState.InProgress;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }
    // Start is called before the first frame update
    void Start()
    {      
        winScreen.SetActive(false);
        time = DateTime.UtcNow;
        levelText.GetComponent<TextMeshProUGUI>().text = levelName;
        _player.onPlayerDissolveComplete += _player_onPlayerDissolved;
    }

    private void _player_onPlayerDissolved(bool isAlive)
    {
        if (!isAlive)
        {
            this.levelState = LevelState.Lost; // irrelevant now
            SceneLoader.RestartLevel();

        } else
        {
            winScreen.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
  
        
        if (levelState != LevelState.InProgress) {
            return;
        }

        elapsedTime = (float)(DateTime.UtcNow - time).TotalSeconds;

        // TODO: Try and void getComponent in an update, it's really slow.
        levelTime.GetComponent<TextMeshProUGUI>().text = String.Format("{0:F2}", (Mathf.Round(elapsedTime * 100.0f ) / 100.0f));
        levelTimeCompleted.GetComponent<TextMeshProUGUI>().text = "total time " + Mathf.Round(elapsedTime * 100.0f) / 100.0f + " seconds";

        if (Input.GetKeyDown(KeyCode.R))
        {
            restartPressedAt = DateTime.UtcNow;
        }

        
        slider.value = (float)(DateTime.UtcNow - restartPressedAt).TotalSeconds / restartHoldDuration;
        
        if (Input.GetKey(KeyCode.R))
        {
            double timeSinceSliderPressed = (DateTime.UtcNow - restartPressedAt).TotalSeconds;
            if (timeSinceSliderPressed >= restartHoldDuration)
            {
                SceneLoader.RestartLevel();
            }
        }
  
    }

    public void Win()
    {
        if (levelState == LevelState.Won)
        {
            Debug.LogWarning("You are calling Win() after the level is already complete. Please Stop!");
            return;
        }

        this.levelState = LevelState.Won;

        _player.Dissolve();
        AudioManager.instance.PlaySound(Sound.Name.LevelComplete);
    }


    public void StopTimer()
    {

    }


}
