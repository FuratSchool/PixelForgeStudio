using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Credits : MonoBehaviour
{
    [SerializeField] private GameObject _credits;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private GameObject _CreditsButton;
    
    public void EndCredits()
    {
        _credits.SetActive(false);
        _mainMenu.SetActive(true);
        _eventSystem.SetSelectedGameObject(null);
        _eventSystem.SetSelectedGameObject(_CreditsButton);
    }
}
