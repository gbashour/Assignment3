using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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