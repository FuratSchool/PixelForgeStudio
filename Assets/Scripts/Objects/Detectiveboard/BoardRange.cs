using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardRange : MonoBehaviour
{
    private BoardController _boardController;
    private UIController _uiController;
    private PlayerController _playerController;
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _boardController = FindObjectOfType<BoardController>();
        _uiController = FindObjectOfType<UIController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _boardController.isPlayerInRange = true;
            _playerController.InteractableRange = true;
            if(other.transform.parent != null) return;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        _boardController.isPlayerInRange = false;
        _playerController.InteractableRange = false;
        if (_boardController.BoardUIActive)
        {
            _boardController.DeactivateBoardUI();
        }
    }
}
