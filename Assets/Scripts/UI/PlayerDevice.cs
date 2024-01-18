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
        foreach (var text in TutorialTexts)
        {
            text.SendMessage("ControlChanged", input, SendMessageOptions.DontRequireReceiver);
        }
        
        var Interactables = GameObject.FindGameObjectsWithTag("Interactable");
        foreach (var text in Interactables)
        {
            text.SendMessage("ChangeText", input, SendMessageOptions.DontRequireReceiver);
        }
        if (input.currentControlScheme.Equals("Controller"))
            Cursor.visible = false;
        else
            Cursor.visible = true;
    }
}
