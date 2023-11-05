using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus playerStatus;
    private int coins = 0;
    
    private Vector3 playerSpawnPoint = new(-3.84f, -0.3f, -54.38f);
    private int Health { get; }

    private void Awake()
    {
        if (playerStatus == null)
        {
            playerStatus = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int getPlayerHealth()
    {
        return Health;
    }

    public void SetSpawnPoint(Vector3 spawnPosition)
    {
        playerSpawnPoint = spawnPosition;
    }

    public Vector3 GetSpawnPoint()
    {
        return playerSpawnPoint;
    }
    public void AddCoin()
    {
        coins++;
        FindObjectOfType<UIController>().SetCoinText(coins);
    }
}