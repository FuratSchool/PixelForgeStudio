using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus playerStatus;
    private int coins = 0;
    
    private Vector3 playerSpawnPoint;
    private int Health { get; }
    
    public int getPlayerHealth()
    {
        return Health;
    }
    public int Coins
    {
        get => coins;
    }
    public void SetSpawnPoint(Vector3 spawnPosition)
    {
        playerSpawnPoint = spawnPosition;
    }

    public Vector3 GetSpawnPoint()
    {
        loseCoin();
        return playerSpawnPoint;
    }
    public void AddCoin()
    {
        coins++;
        FindObjectOfType<UIController>().SetCoinText(coins);
    }
    
    public void loseCoin()
    {
        if (coins > 0)
        {
            coins -= Random.Range(2, 4);
            FindObjectOfType<UIController>().SetCoinText(coins);
        }
        if(coins <= 0)
        {
            coins = 0;
            FindObjectOfType<UIController>().SetCoinText(coins);
        }
        
    }
}