using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsPlayerDeath : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().touchedWater = true;
        }
    }
}
