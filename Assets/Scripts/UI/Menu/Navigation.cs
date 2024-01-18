using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Serialization;

public class Navigation : MonoBehaviour
{
    private string ActiveControlScheme;
    [SerializeField] private GameObject fistMainMenu;
    public bool inMenu;
    public GameObject LoadingScreen;

    private void Update()
    {
        if(!inMenu) return;
        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(fistMainMenu);
    }
}
