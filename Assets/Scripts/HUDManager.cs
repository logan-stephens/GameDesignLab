using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    // public variables:
    public GameObject scoreText;
    public Transform restartButton;
    public GameObject gameOverPanel;

    public GameObject highscoreText;
    public IntVariable gameScore;

    // positions:
    private Vector3[] scoreTextPosition = {
        new Vector3(-900, 600, 0),
        new Vector3(0, 0, 0)
        };
    private Vector3[] restartButtonPosition = {
        new Vector3(1000, 600, 0),
        new Vector3(0, -150, 0)
    };

    void Awake()
    {
        GameManager.instance.gameStart.AddListener(GameStart);
        GameManager.instance.gameOver.AddListener(GameOver);
        GameManager.instance.gameRestart.AddListener(GameStart);
        GameManager.instance.scoreChange.AddListener(SetScore);

    }

    void Start()
    {
        GameStart();
        SetScore(GameManager.instance.gameScore.Value);
    }

    public void GameStart()
    {
        Debug.Log("HUDManager GameStart called");
        // hide gameover panel
        gameOverPanel.SetActive(false);
        scoreText.transform.localPosition = scoreTextPosition[0];
        restartButton.localPosition = restartButtonPosition[0];
    }

    public void SetScore(int score)
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }


    public void GameOver()
    {
        Debug.Log("HUDManager GameOver called");

        highscoreText.GetComponent<TextMeshProUGUI>().text = "TOP- " + gameScore.previousHighestValue.ToString("D6");
        highscoreText.SetActive(true);

        gameOverPanel.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[1];
        restartButton.localPosition = restartButtonPosition[1];
    }
}
