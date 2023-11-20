using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    private string current;
    private string last;
    [SerializeField] private TMP_Text _interactableTextElement;
    [SerializeField] private TMP_Text _coinTextElement;
    [SerializeField] private TMP_Text _FinishTextElement;
    [SerializeField] private PlayerInput InputAction;

    private void Update()
    {
        if (InputAction == null) return;
        current = InputAction.currentControlScheme;
        if (current != last)
        {
            Debug.Log(current);
            last = current;
        }
    }

    public void SetInteractText(string text)
    {
        _interactableTextElement.text = text;
    }
    
    public void SetInteractableTextActive(bool active)
    {
        _interactableTextElement.gameObject.SetActive(active);
    }
    
    public void SetCoinTextActive(bool active)
    {
        _coinTextElement.gameObject.SetActive(active);
    }
    
    public void SetCoinText(int coins)
    {
        _coinTextElement.text = Convert.ToString(coins);
    }
    
    public void SetFinishActive(bool active)
    {
        _FinishTextElement.gameObject.SetActive(active);
    }
}
