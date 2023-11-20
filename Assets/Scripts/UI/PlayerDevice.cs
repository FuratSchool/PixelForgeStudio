using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDevice : MonoBehaviour
{
    private string ActiveControlScheme;
    public void OnControlsChanged()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        var TutorialTexts = GameObject.FindGameObjectsWithTag("ControlText");
        Debug.Log(input.currentControlScheme);
        foreach (var text in TutorialTexts)
        {
            text.SendMessage("ControlChanged", input, SendMessageOptions.DontRequireReceiver);
        }
    }
}
