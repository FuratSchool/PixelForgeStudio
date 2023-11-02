using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingableObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<Swinging>().InRange = true;
        FindObjectOfType<Swinging>().SwingableObjectGAME = transform.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        FindObjectOfType<Swinging>().InRange = false;
    }
}
