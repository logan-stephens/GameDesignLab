using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuNavigation : MonoBehaviour
{
    public IntVariable gameScore;
    public TextMeshProUGUI highscoreText;
    public static string nextLevel = "World-1-1";
    public TextMeshProUGUI worldTextTop;
    public TextMeshProUGUI worldTextCenter;

    void Start()
    {
        string displayWorld = nextLevel.Replace("World-", "WORLD ");

        if (worldTextTop != null) worldTextTop.text = displayWorld;

        if (worldTextCenter != null) worldTextCenter.text = displayWorld;

        if (gameScore != null && highscoreText != null) UpdateHighscore();

        if (SceneManager.GetActiveScene().name == "LoadingScene") StartCoroutine(LoadAfterDelay());
    }

    IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSecondsRealtime(2f);
        StartLevel();
    }

    public void PlayGame()
    {
        LoadLevel("World-1-1");
    }

    public void StartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextLevel);
    }

    public static void LoadLevel(string levelName)
    {
        nextLevel = levelName;
        Time.timeScale = 1f;
        SceneManager.LoadScene("LoadingScene");
    }

    public void BackToMenu()
    {
        Debug.Log("Attempted return");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetHighscore()
    {
        gameScore.ResetHighestValue();
        UpdateHighscore();
    }

    void UpdateHighscore()
    {
        highscoreText.text = "TOP- " +
            gameScore.previousHighestValue.ToString("D6");
    }
}
