using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private readonly List<string> tagList = new() { "platform", "Checkpoint" };

    private void OnCollisionEnter(Collision collision)
    {
        if (tagList.Contains(collision.gameObject.tag))
        {
            Debug.Log("Triggered");
            transform.SetParent(collision.transform);
            transform.GetComponent<PlayerController>().raycastDistance = 2.8f;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        transform.parent = null;
        transform.GetComponent<PlayerController>().raycastDistance = 1.7f;
    }
}