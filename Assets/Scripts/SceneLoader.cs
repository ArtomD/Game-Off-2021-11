using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    public void Awake()
    {
                
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("Main");
    }

    public static void LoadSettings()
    {
        SceneManager.LoadScene("Options");
    }

    public static void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void LoadFirstLevel()
    {
        SceneManager.LoadScene(3);
    }

    public static void LoadIntro()
    {
        SceneManager.LoadScene("Intro");
        AudioManager.instance.PlaySound(Sound.Name.Fail);
    }

    public static void LoadLevel(int i)
    {
        SceneManager.LoadScene(i);
    }
}
