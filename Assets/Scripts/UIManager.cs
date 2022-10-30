using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI score;
    public TMPro.TextMeshProUGUI gameTimer;
    public TMPro.TextMeshProUGUI ghostScaredTimer;
    public TMPro.TextMeshProUGUI roundTimer;
    float timer, startTimer;
    int minutes, seconds, milliseconds;
    int lastTime = 0, countdown = 3;
    /* Text type throws a Null Reference Exception, doesn't register TextMeshPro as type Text */
    public TMPro.TextMeshProUGUI highScore; // after testing, figured out that the type "Text" doesn't work because I'm using TextMeshPro
    public TMPro.TextMeshProUGUI time;

    private static bool roundStart = false;
    public static bool RoundStart
    {
        get { return roundStart; }
    }

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
        if (SceneManager.GetActiveScene().buildIndex == 0) // if Level1Scene is the active scene
        {
            startTimer += Time.deltaTime;
            if (roundStart)
            {
                /* GAME TIMER */
                timer += Time.deltaTime; // this is in seconds
                int milliTimer = (int)(timer * 1000.0f); // total milliseconds
                minutes = milliTimer / 60000; // how many minutes = total milliseconds / 60000
                seconds = (int)timer - (minutes * 60);
                milliseconds = (milliTimer - (minutes * 60000) - (seconds * 1000)) / 10;
                gameTimer.text = "Timer: " + minutes + ":" + seconds + ":" + milliseconds; // update the timer every frame
                int totalScore = PacStudentController.PacStudentScore + CherryController.PeachScore; // store total score in separate variable because otherwise it just concatenates them like strings
                score.text = "Score: " + totalScore; // update score every frame
                if (PacStudentController.GhostTimer < 11 && PacStudentController.GhostTimer > -1)
                {
                    ghostScaredTimer.text = PacStudentController.GhostTimer + " seconds";

                }
                else
                {
                    ghostScaredTimer.text = "";
                }
            }

            if ((int)startTimer - lastTime == 1) // every second
            {
                Debug.Log("I should be printing to the screen");
                lastTime = (int)startTimer;

                if (lastTime < 4 && lastTime > 0)
                {
                    roundTimer.text = countdown.ToString();
                    countdown--;
                } else if (lastTime == 4)
                {
                    roundTimer.text = "GO!";
                } else
                {
                    roundTimer.text = "";
                    roundStart = true;
                }
            }
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
            if (roundStart)
            {
                roundStart = false;
            }
            Button button = GameObject.FindGameObjectWithTag("ExitButton").GetComponent<Button>();
            button.onClick.AddListener(ExitGame);

            gameTimer = GameObject.FindGameObjectWithTag("GameTimer").GetComponent<TMPro.TextMeshProUGUI>();
            score = GameObject.Find("Score").GetComponent<TMPro.TextMeshProUGUI>();
            ghostScaredTimer = GameObject.Find("Ghost Scared Timer").GetComponent<TMPro.TextMeshProUGUI>();
            roundTimer = GameObject.Find("Round Timer").GetComponent<TMPro.TextMeshProUGUI>();
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