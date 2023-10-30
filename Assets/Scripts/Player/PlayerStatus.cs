using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus playerStatus;

    private Vector3 playerSpawnPoint = new(0, 0, 0);
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
}