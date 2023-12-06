using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisablePlayerInput : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject _player;
    private bool _PlayerFound;
    private void OnEnable()
    {
        if (GameObject.Find("PlayerObject") != null)
        {
            _PlayerFound = true;
            _player = GameObject.Find("PlayerObject");
            _player.SetActive(false);
        }
        else
        {
            _PlayerFound = false;
        }
        FindObjectOfType<PlayerInput>().enabled = false;
    }

    private void OnDisable()
    {
        if (_PlayerFound)
        {
            _player.SetActive(true);
        }
        FindObjectOfType<PlayerInput>().enabled = true;
    }
}
