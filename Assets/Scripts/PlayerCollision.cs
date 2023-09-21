using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private List<string> tagList = new List<string>() { "platform", "Checkpoint" }; 
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (tagList.Contains(collision.gameObject.tag))
        {
            Debug.Log("Triggered");
            transform.SetParent(collision.transform);
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        transform.parent = null;
    }
}