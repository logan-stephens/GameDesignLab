using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public void GameRestart()
    {
        Debug.Log("EnemyManager GameRestart called");
        foreach (Transform child in transform)
        {
            child.GetComponent<EnemyMovement>().GameRestart();
        }
    }

    void Awake()
    {
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }
}
