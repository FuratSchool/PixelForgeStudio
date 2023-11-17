using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisablePlayerInput : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        FindObjectOfType<PlayerInput>().enabled = false;
    }

    private void OnDisable()
    {
        FindObjectOfType<PlayerInput>().enabled = true;
    }
}
