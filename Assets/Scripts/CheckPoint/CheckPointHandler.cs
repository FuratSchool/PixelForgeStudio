using UnityEngine;

public class CheckPointHandler : MonoBehaviour
{
    private PlayerStatus playerStatus;
    private bool triggered;

    private void Start()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            Debug.Log("Player entered the checkpoint");
            SavePlayerProgress();
            triggered = true;
            DisplayCheckpointActivatedMessage();

            // Reset the player's spawn point to the checkpoint's position.
            if (playerStatus != null)
            {
                Debug.Log("test");
                playerStatus.SetSpawnPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z));
            }
        }
    }

    private void SavePlayerProgress()
    {
    }

    private void DisplayCheckpointActivatedMessage()
    {
    }

    public void OnEnterCheckpoint()
    {
        if (!triggered)
        {
            Debug.Log("Player entered the checkpoint");
            SavePlayerProgress();
            triggered = true;
            DisplayCheckpointActivatedMessage();
        }
    }

    public void OnExitCheckpoint()
    {
        Debug.Log("Player exited the checkpoint");
    }
}