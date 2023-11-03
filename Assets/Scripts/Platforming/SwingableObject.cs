using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingableObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<SwingingController>().InRange = true;
        FindObjectOfType<SwingingController>().SwingableObjectGAME = transform.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        FindObjectOfType<SwingingController>().InRange = false;
    }
}
