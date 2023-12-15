using UnityEngine;

public class CheckPointHandler : MonoBehaviour
{
    private PlayerStatus playerStatus;
    private bool triggered;
    [SerializeField] private GameObject checkpointFire;
    [SerializeField] private GameObject checkpointLight;
    [SerializeField] private GameObject RespawnPoint;

    private void Start()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();
        checkpointFire.SetActive(false);
        checkpointLight.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            Debug.Log("Player entered the checkpoint");
            SavePlayerProgress();
            triggered = true;
            DisplayCheckpointActivatedMessage();
            checkpointFire.SetActive(true);
            checkpointLight.SetActive(true);
            // Reset the player's spawn point to the checkpoint's position.
            if (playerStatus != null)
            {
                Debug.Log("test");
                playerStatus.SetSpawnPoint(RespawnPoint.transform.position);
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