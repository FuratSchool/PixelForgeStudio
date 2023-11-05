using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingableObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<PlayerController>().inSwingingRange = true;
        FindObjectOfType<PlayerController>().SwingableObjectGAME = transform.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        FindObjectOfType<PlayerController>().inSwingingRange = false;
    }
}
