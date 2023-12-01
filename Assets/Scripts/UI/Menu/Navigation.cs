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

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null && ActiveControlScheme.Equals("Controller"))
            EventSystem.current.SetSelectedGameObject(fistMainMenu);
    }

    public void OnOptionsEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOptionsMenu);
    }
    
    public void OnOptionsDisable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(BackOptionsMenu);
    }

    public void OnControlsChanged()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        ActiveControlScheme = input.currentControlScheme;
    }
}
