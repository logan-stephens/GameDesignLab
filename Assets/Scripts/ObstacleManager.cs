using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public void GameRestart()
    {
        // Loop through all child obstacles/blocks and reset them
        foreach (Transform child in transform)
        {
            Block block = child.GetComponentInChildren<Block>();
            if (block != null) block.GameRestart();
        }
    }
}