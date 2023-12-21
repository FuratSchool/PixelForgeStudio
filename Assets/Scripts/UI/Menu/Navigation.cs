using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Navigation : MonoBehaviour
{
    private string ActiveControlScheme;
    [SerializeField] private GameObject fistMainMenu;
    [SerializeField] private GameObject firstOptionsMenu;
    [SerializeField] private GameObject BackOptionsMenu;
    private bool Options;
    private void Update()
    {
        if(!Options)
            if (EventSystem.current.currentSelectedGameObject == null && ActiveControlScheme.Equals("Controller"))
                EventSystem.current.SetSelectedGameObject(fistMainMenu);
    }

    public void OnOptionsEnable()
    {
        Options = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOptionsMenu);
    }
    
    public void OnOptionsDisable()
    {
        Options = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(BackOptionsMenu);
    }

    public void OnControlsChanged()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        ActiveControlScheme = input.currentControlScheme;
        if (ActiveControlScheme.Equals("Controller"))
            Cursor.visible = false;
        else
            Cursor.visible = true;
    }
}
