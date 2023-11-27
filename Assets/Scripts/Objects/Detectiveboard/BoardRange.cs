using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardRange : MonoBehaviour
{
    private BoardController _boardController;
    private UIController _uiController;
    void Start()
    {
        _boardController = FindObjectOfType<BoardController>();
        _uiController = FindObjectOfType<UIController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_boardController.debugBoard) Debug.Log("Player entered");
            _boardController.isPlayerInRange = true;
            if(other.transform.parent != null) return;
            EnableInteractActive(_uiController, other.GetComponent<PlayerInput>());
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (_boardController.debugBoard) Debug.Log("Player Exited");
        _boardController.isPlayerInRange = false;
        DisableInteractActive(_uiController);
        if (_boardController.BoardUIActive)
        {
            _boardController.DeactivateBoardUI();
        }
    }
    
    public void EnableInteractActive(UIController uiController, PlayerInput playerInput)
    {
        string keybind;
        if (playerInput.currentControlScheme.Equals("Controller"))
        {
            int index = playerInput.actions["Interact"].bindings.IndexOf(x => x.groups.Contains("Controller"));
            keybind = playerInput.actions["Interact"].GetBindingDisplayString(index, out var deviceLayoutName, out var controlPath);
        }
        else
        {
            int index = playerInput.actions["Interact"].bindings.IndexOf(x => x.groups.Contains("KeyboardMouse"));
            keybind = playerInput.actions["Interact"].GetBindingDisplayString(index, out var deviceLayoutName, out var controlPath);
        }
        uiController.SetInteractText("Press "+ keybind +" to interact");
        uiController.SetInteractableTextActive(true);
    }
    public void DisableInteractActive(UIController uiController)
    {
        uiController.SetInteractableTextActive(false);
    }
}
