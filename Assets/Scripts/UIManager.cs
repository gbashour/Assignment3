using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI gameTimer;
    float timer;
    int minutes, seconds, milliseconds;
    /* Text type throws a Null Reference Exception, doesn't register TextMeshPro as type Text */
    public TMPro.TextMeshProUGUI highScore; // after testing, figured out that the type "Text" doesn't work because I'm using TextMeshPro
    public TMPro.TextMeshProUGUI time;

    // Start is called before the first frame update
    void Start()
    {
        highScore = GameObject.Find("HighScore").GetComponent<TMPro.TextMeshProUGUI>();
        // highScore.text = "Test"; // it works! -- still need to implement PlayerPrefs

        time = GameObject.Find("Time").GetComponent<TMPro.TextMeshProUGUI>();
        // time.text = "Time: " + PlayerPrefs.GetFloat(); -- to be implemented
    }

    // Update is called once per frame
    void Update()
    {
        /* GAME TIMER */
        timer += Time.deltaTime; // this is in seconds
        int milliTimer = (int)(timer * 1000.0f); // total milliseconds
        minutes = (int)Mathf.Ceil(milliTimer / 60000); // how many minutes = total milliseconds / 60000
        seconds = (int)timer - (minutes * 60);
        milliseconds = milliTimer - (minutes * 60000) - (seconds * 1000);

        if (SceneManager.GetActiveScene().buildIndex == 0) // if Level1Scene is the active scene
        {
            gameTimer.text = "Timer: " + minutes + ":" + seconds + ":" + milliseconds; // update the timer every frame
        }
    }

    public void LoadFirstLevel()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Level1Scene");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) // if Level1Scene is the current scene
        {
            Button button = GameObject.FindGameObjectWithTag("ExitButton").GetComponent<Button>();
            button.onClick.AddListener(ExitGame);

            gameTimer = GameObject.FindGameObjectWithTag("GameTimer").GetComponent<TMPro.TextMeshProUGUI>();
        }
    }

    public void ExitGame()
    {
        /* BUG FIX: DontDestroyOnLoad() was creating duplicate Managers game objects
           Destroying the existing game object when returning to StartScene fixes this issue */
        Destroy(gameObject);
        SceneManager.LoadScene("StartScene");
    }
}