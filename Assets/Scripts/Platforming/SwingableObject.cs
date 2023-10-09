using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingableObject : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Swinging swingingComponent = other.GetComponent<Swinging>();
        if (swingingComponent != null)
        {
            swingingComponent.inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Swinging swingingComponent = other.GetComponent<Swinging>();
        if (swingingComponent != null)
        {
            swingingComponent.inRange = true;
        }
    }
}
