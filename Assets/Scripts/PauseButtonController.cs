using UnityEngine;
using UnityEngine.UI;

public class PauseButtonController : MonoBehaviour, IInteractiveButton
{
    private bool isPaused = false;

    public Sprite pauseIcon;
    public Sprite playIcon;
    public GameObject pauseOverlay;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();

        pauseOverlay.SetActive(false);
        image.sprite = pauseIcon;
    }

    public void ButtonClick()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        AudioListener.pause = isPaused;

        image.sprite = isPaused ? playIcon : pauseIcon;
        pauseOverlay.SetActive(isPaused);
    }
}