using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMovement : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<PlayerController>().EnableMovement = true;
        }
    }
}
