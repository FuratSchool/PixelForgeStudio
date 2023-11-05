using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private bool _hasFinished = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<UIController>().SetFinishActive(true);
            _hasFinished = true;
        }
    }

    private void Update()
    {
        if (_hasFinished && Input.GetKey(KeyCode.F12))
        {
            FindObjectOfType<SceneController>().LoadScene("MainMenu");
        }
    }
}
