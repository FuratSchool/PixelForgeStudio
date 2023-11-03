using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _interactableTextElement;
    [SerializeField] private TMP_Text _coinTextElement;
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
}
