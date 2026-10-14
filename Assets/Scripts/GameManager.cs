using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // UnityEvents
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent gameOver;

    // public variables
    public IntVariable gameScore;

    // private variables
    // private int score = 0;


    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;

        SceneManager.activeSceneChanged += SceneSetup;
    }

    public void SceneSetup(Scene current, Scene next)
    {
        gameStart.Invoke();
        SetScore(gameScore.Value);
    }

    public void GameRestart() 
    {
        Debug.Log("GameRestart called");
        // reset score
        gameScore.Value = 0;
        SetScore(gameScore.Value);
        Debug.Log("SetScore succeeded");
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
    }

    public void IncreaseScore(int increment)
    {
        gameScore.ApplyChange(increment);
        SetScore(gameScore.Value);
    }

    public void SetScore(int score)
    {
        scoreChange.Invoke(score);
    }


    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOver.Invoke();
    }
}