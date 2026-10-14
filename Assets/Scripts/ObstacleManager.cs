using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public void GameRestart()
    {
        Debug.Log("ObstacleManager GameRestart called");
        // Loop through all child obstacles/blocks and reset them
        foreach (Transform child in transform)
        {
            Block block = child.GetComponentInChildren<Block>();
            if (block != null) block.GameRestart();

            QuestionBoxPowerupController questionBox = child.GetComponentInChildren<QuestionBoxPowerupController>();
            if (questionBox != null) questionBox.GameRestart();
        }
    }

    void Awake() {
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }
}