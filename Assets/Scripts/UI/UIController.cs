using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private string current;
    private string last;
    [SerializeField] private TMP_Text _interactableTextElement;
    [SerializeField] private TMP_Text _coinTextElement;
    [SerializeField] private GameObject _coinGameObject;
    [SerializeField] private TMP_Text _FinishTextElement;
    [SerializeField] private PlayerInput InputAction;
    [SerializeField] public float FadeTime = 1;
    [SerializeField] private float FadeDelay = 2;
    private bool fadeActive = false;

    private void Start()
    {
        SetCoinAlpha(0);
    }

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
        FadeOutCoin();
    }

    public void SetCoinAlpha(float Alpha)
    {
        _coinGameObject.GetComponent<CanvasRenderer>().SetAlpha(Alpha);
        _coinTextElement.GetComponent<CanvasRenderer>().SetAlpha(Alpha);
    }

    private void FadeOutCoin()
    {
        SetCoinAlpha(1);
        if (fadeActive)
        {
            StopCoroutine(Wait());
            StartCoroutine(Wait());
        }
        else
        {
            StartCoroutine(Wait());
        }
    }
    
    
    IEnumerator Wait()
    {
        fadeActive = true;
        yield return new WaitForSeconds(FadeDelay);
        AplhaCoins(0, FadeTime, false);
    }
    
    private void AplhaCoins(float alpha, float duration, bool timescale)
    {
        _coinGameObject.GetComponent<RawImage>().CrossFadeAlpha(alpha,duration,timescale);
        _coinTextElement.CrossFadeAlpha(alpha,duration,timescale);
    }
    public void SetFinishActive(bool active)
    {
        _FinishTextElement.gameObject.SetActive(active);
    }
}
