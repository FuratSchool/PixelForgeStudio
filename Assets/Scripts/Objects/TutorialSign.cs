using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]
public class TutorialSign : MonoBehaviour
{
    
    private bool _inRange;
    private PlayerController _playerController;
    private PauseMenu _pauseMenu;
    private bool _signActive;
    
    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _pauseMenu = FindObjectOfType<PauseMenu>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _inRange = true;
            _playerController.InteractableRange = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _inRange = false;
            _playerController.InteractableRange = false;
        }
    }
    
    public void InInteractState(PlayerController pc)
    {
        if (_inRange)
        {
            if (_signActive)
            {
                _signActive = false;
                _pauseMenu.CloseSign();
                GetComponentInChildren<TutorialKeybindings>().SignActive = false;
            }
            else
            {
                _pauseMenu.OpenSign(GetComponentInChildren<TutorialKeybindings>().TutorialText);
                _signActive = true;
                GetComponentInChildren<TutorialKeybindings>().SignActive = true;
            }
        }
    }

    public void updateText(string text)
    {
        _pauseMenu.UpdateSignText(text);
    }
}
